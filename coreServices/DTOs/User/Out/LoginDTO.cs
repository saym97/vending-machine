using coreServices.Enums;

namespace coreServices.DTOs.User.Out
{
    public class LoginDTO : GenericResponse
    {
        public string Username { get; set; }
        public byte Role { get; set; }
        public int Deposit { get; set; }
        public JwToken JwToken { get; set; }
        
    }
}
