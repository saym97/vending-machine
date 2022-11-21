using AutoMapper;
using coreServices.DTOs.User.In;
using coreServices.Services.User;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPatch("update-password")]
        [Authorize]
        public IActionResult UpdatePassword([FromBody] string password)
        {
            var loggedUser = GetLoggedUser();
            if (loggedUser == null)
                return Unauthorized("Token is invalid");

            if (password.IsNullOrEmpty())
                return BadRequest("password cannot be empty");

            var result = _userService.UpdatePassword(loggedUser.Id, password);

            if (result.Success)
                return Ok(result.Message);

            return NotFound(result.Message);

        }

        [HttpPatch("update-username")]
        [Authorize]
        public IActionResult UpdateUsername([FromBody] string username)
        {
            var loggedUser = GetLoggedUser();
            if(loggedUser == null)
                return Unauthorized("Token is invalid");

            if (username.IsNullOrEmpty())
                return BadRequest("username cannot be empty");

            var result = _userService.UpdateUsername(loggedUser.Id, username);

            if (result.Success)
                return Ok(result.Message);

            return NotFound(result.Message);

        }

        [HttpPatch("deposit")]
        [Authorize(Roles = "Buyer")]
        public IActionResult DepositMoney([FromBody] int amount)
        {
            var loggedUser = GetLoggedUser();
            if (loggedUser == null)
                return Unauthorized("Token is invalid");

            if (amount % 5 != 0)
                return BadRequest("You can only deposit the coins of 5, 10, 20, 50 and 100 cents");

            var result = _userService.Deposit(loggedUser.Id, amount);

            if (result.Success)
                return Ok(result.Message);

            return NotFound(result.Message);
        }
    }
}