using AutoMapper;
using coreServices.DTOs;
using coreServices.DTOs.User;
using coreServices.DTOs.User.In;
using coreServices.Enums;
using coreServices.Helper;
using coreServices.Infrastructure.Base;
using dbContext.VendingMachine;
using dbContext.VendingMachine.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace coreServices.Services.User
{
    public class UserService : BaseService, IUserService
    {
        private readonly IMapper _mapper;
        private readonly VendingMachineContext _dbContext;
        private IConfiguration _configuration;
        private List<int> coinsAllowed = new List<int>() { 5, 10, 20, 50, 100 };
        public UserService(VendingMachineContext dbContext, IMapper mapper, IConfiguration configuration) : base(mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
        }


        public GenericResponse Register(SignupDTO signupCredentials)
        {
            var retval = new GenericResponse()
            {
                Success = false,
                Message = "There was an error while registration, try again"
            };

            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                bool userExistsAlready = _dbContext.Users.Any(x=> x.Username == signupCredentials.Username);
                if (userExistsAlready)
                {
                    throw new Exception("user exists already");
                }

                string salt;
                string hash;

                PasswordHelper.GenerateHash(signupCredentials.Password, out salt, out hash);

                var user = new Users()
                {
                    Username = signupCredentials.Username,
                    Salt = salt,
                    Hash = hash,
                    Role = signupCredentials.Role,
                };

                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                transaction.Commit();
                retval.Success = true;
                retval.Message = $"The user: {signupCredentials.Username} was created successfully.";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                retval.Message = ex.Message;
                return retval;
            }

            return retval;
        }

        public DTOs.User.Out.LoginDTO Authenticate(DTOs.User.In.LoginDTO loginCredentials)
        {
            var retval = new DTOs.User.Out.LoginDTO()
            {
                Success = false,
                Message = "There was an error while logging in. Try again"
            };

            var user = _dbContext.Users.FirstOrDefault(x => x.Username == loginCredentials.Username);
            if(user == null)
            {
                retval.Message = "User doesn't exist";
                return retval;
            }
            
            bool isPasswordCorrect = PasswordHelper.VerifyPasswordHash(loginCredentials.Password,user.Salt,user.Hash);

            if (!isPasswordCorrect)
            {
                retval.Message = "Invalid username or password. Try again";
                return retval;
            }

            retval.Username = user.Username;
            retval.Role = user.Role;
            retval.Deposit = user.Deposit;
            retval.Success = true;
            retval.Message = "Logged In successfully";
            retval.JwToken = GenerateJwToken(user);

            return retval;
        }


        public GenericResponse Deposit(Guid userId, int amount)
        {
            var retval = new GenericResponse()
            {
                Success = false,
                Message = "Error occured while depositing"
            };

            bool IsCoinAllowed = coinsAllowed.Any(x=> x == amount);
            if (!IsCoinAllowed)
            {
                retval.Message = "You can only deposit the coins of 5, 10, 20, 50 and 100 cents";
                return retval;
            }
            var user = _dbContext.Users.FirstOrDefault(x => x.UserId.Equals(userId));
            if (user == null)
                return retval;

            user.Deposit += amount;
            _dbContext.SaveChanges();
            retval.Success = true;
            retval.Message = $"Deposit successful, you current balance is {user.Deposit}";
            return retval;
        }

        public GenericResponse Reset(Guid userId)
        {
            var retval = new GenericResponse()
            {
                Success = false,
                Message = "Error occured while reseting the deposit amount"
            };

            var user = _dbContext.Users.FirstOrDefault(x => x.UserId.Equals(userId));

            if (user == null)
                return retval;

            user.Deposit = 0;
            _dbContext.SaveChanges();

            retval.Success = true;
            retval.Message = "Your deposits were reset successfully.";

            return retval;
        }

        public GenericResponse UpdatePassword(Guid userId,string password)
        {
            var retval = new GenericResponse()
            {
                Success = false,
                Message = "Error occured while updating the password, try again"
            };

            var user = _dbContext.Users.FirstOrDefault(x=>x.UserId.Equals(userId));
            if(user == null)
                return retval;

            string salt, hash;
            PasswordHelper.GenerateHash(password, out salt, out hash);
            user.Salt = salt;
            user.Hash = hash;
            _dbContext.SaveChanges();
            retval.Success = true;
            retval.Message = $"Successfully change the password for user {user.Username}";
            return retval;
        }

        public GenericResponse UpdateUsername(Guid userId, string username)
        {
            var retval = new GenericResponse()
            {
                Success = false,
                Message = "Error occured while updating the username, try again"
            };

            var UserExistAlready = _dbContext.Users.Any(x => x.Username == username);
            if (UserExistAlready)
            {
                retval.Message = $"The username {username} is already taken";
                return retval;
            }

            var user = _dbContext.Users.FirstOrDefault(x => x.UserId.Equals(userId));
            if (user == null)
                return retval;

            user.Username = username;
            _dbContext.SaveChanges();
            retval.Success = true;
            retval.Message = $"Successfully change the username for user {user.Username}";
            return retval;
        }


        private JwToken GenerateJwToken(Users user)
        {
            if (user == null)
                return null;

            var secret = Encoding.ASCII.GetBytes(_configuration.GetSection("JWTSecretKey").Value);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature);
            var expires = DateTime.UtcNow.AddHours(2);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.PrimarySid,user.UserId.ToString()),
                new Claim(ClaimTypes.NameIdentifier,user.Username),
            };
            
            if(user.Role == (byte)UserRoleEnum.Seller)
                claims.Add(new Claim(ClaimTypes.Role,"Seller"));
            
            if(user.Role == (byte)UserRoleEnum.Buyer)
                claims.Add(new Claim(ClaimTypes.Role, "Buyer"));

            var token = new JwtSecurityToken(
                claims : claims,
                expires : expires,
                signingCredentials: credentials
                );

            var Jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return new JwToken()
            {
                Token = Jwt,
                Expires = expires
            };
        }

        public CurrentUserDTO GetAuthenticatedUser(ClaimsIdentity? identity)
        {
            if (identity == null)
                return null;
            var retval = new CurrentUserDTO();
            IEnumerable<Claim>  claims = identity.Claims;
            var guid = claims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value;
            var  username = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var role = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

            if(guid != null)
                retval.Id = new Guid(guid);

            if (username != null)
                retval.Username = username;

            if (role.Equals("Seller"))
                retval.Role = UserRoleEnum.Seller;

            if (role.Equals("Buyer"))
                retval.Role = UserRoleEnum.Buyer;

            return retval;
        }
    }
}
