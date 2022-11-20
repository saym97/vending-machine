using coreServices.Enums;

namespace coreServices.DTOs.User
{
    public class CurrentUserDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public UserRoleEnum Role { get; set; }
    }
}
