using AutoMapper;
using coreServices.DTOs;
using coreServices.DTOs.User;
using coreServices.DTOs.User.In;
using coreServices.Enums;
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

                var user = new Users()
                {
                    Username = signupCredentials.Username,
                    Password = signupCredentials.Password,
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
            
            bool isPasswordCorrect = user.Password == loginCredentials.Password;

            if (!isPasswordCorrect)
            {
                retval.Message = "Invalid username or password. Try again";
                return retval;
            }

            retval.Username = user.Username;
            retval.Role = user.Role;
            retval.Success = true;
            retval.JwToken = GenerateJwToken(user);

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
    }
}
