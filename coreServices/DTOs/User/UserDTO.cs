using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreServices.DTOs.User
{
    public class UserDTO
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Deposit { get; set; }
        public byte Role { get; set; }
    }
}
