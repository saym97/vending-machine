using AutoMapper;
using coreServices.DTOs.User.In;
using coreServices.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : BaseController
    {

        public UserController(IMapper mapper, ILogger<UserController> logger,IUserService userService ): base(mapper,logger,userService){}


        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] LoginDTO credentials)
        {
            //GUARD  exit if request object is invalid 
            if(credentials == null || credentials.Username.IsNullOrEmpty() || credentials.Password.IsNullOrEmpty())
                return BadRequest("Please enter valid credentials");

            var result = _userService.Authenticate(credentials);

            //GUARD exit if user service couldn't verify the user
            if(!result.Success)
                return BadRequest(result.Message);

            return Ok(result);

        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] SignupDTO credentials)
        {
            //GUARD  exit if request object or a must property is invalid 
            if (credentials == null)
                return BadRequest("Please enter valid credentials");
            if (credentials.Username.IsNullOrEmpty())
                return BadRequest("Username cannot be emtpy.");
            if (credentials.Password.IsNullOrEmpty())
                return BadRequest("Password cannot be emtpy.");

            var result = _userService.Register(credentials);

            //GUARD exit if user service couldn't verify the user
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);

        }
    }
}