using coreServices.DTOs;
using coreServices.DTOs.User;
using coreServices.DTOs.User.In;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace coreServices.Services.User
{
    public interface IUserService
    {
        DTOs.User.Out.LoginDTO Authenticate(DTOs.User.In.LoginDTO loginCredentials);
        CurrentUserDTO GetAuthenticatedUser(ClaimsIdentity? identity);
        GenericResponse Register(SignupDTO signupCredentials);
    }
}
