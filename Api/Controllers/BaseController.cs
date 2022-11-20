using AutoMapper;
using coreServices.DTOs.User;
using coreServices.Services.User;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    public class BaseController: ControllerBase
    {
        protected readonly IMapper _mapper;
        protected readonly IUserService _userService;
        protected readonly ILogger _logger;

        public BaseController(IMapper mapper, ILogger logger, IUserService userService)
        {
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
        }

        protected CurrentUserDTO GetLoggedUser()
        {
            var identity = Request.HttpContext.User.Identity as ClaimsIdentity;
            var loggedUser = _userService.GetAuthenticatedUser(identity);
            return loggedUser;
        }
    }
}
