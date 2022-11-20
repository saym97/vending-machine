namespace coreServices.DTOs.User.In
{
    public class SignupDTO
    {
        public string Username { get; set; }

        public string Password { get; set; }
        public byte Role { get; set; }
    }
}
