using coreServices.DTOs.User;
using dbContext.VendingMachine.Entities;

namespace coreServices.AutoMapperProfile
{
    public class UserMappingProfile: AutoMapper.Profile
    {
        public UserMappingProfile()
        {
            CreateMap<Users, UserDTO>();
            CreateMap<UserDTO, Users>();
        }
    }
}
