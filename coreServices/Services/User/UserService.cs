using AutoMapper;
using coreServices.DTOs.User;
using coreServices.Enums;
using coreServices.Infrastructure.Base;
using dbContext.VendingMachine;
using dbContext.VendingMachine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreServices.Services.User
{
    public class UserService : BaseService, IUserService
    {
        private readonly IMapper _mapper;
        private readonly VendingMachineContext _dbContext;
        public UserService(VendingMachineContext dbContext, IMapper mapper) : base(mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public UserDTO HelloUserService()
        {
            UserDTO user = new UserDTO()
            {
                Deposit = 10,
                Password = "12345678",
                Username = "usename@mail.com",
                Role = (byte)UserRoleEnum.Seller
            };
            AddNewUser(user);
            return user;
        }

        private void AddNewUser(UserDTO userDto)
        {
            var user = _mapper.Map<Users>(userDto);
            try
            {
                bool userExistsAlready = _dbContext.Users.Any(x=> x.Username == userDto.Username);
                if (userExistsAlready)
                {
                    throw new Exception("user exists already");
                }
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
