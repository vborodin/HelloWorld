using AuthenticationService.Models;
using AuthenticationService.Services;
using AuthenticationService.TokenGenerator;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ITokenGenerator<UserModel> tokenGenerator;

        public LoginController(IUserService userService, ITokenGenerator<UserModel> tokenGenerator)
        {
            this.userService = userService;
            this.tokenGenerator = tokenGenerator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = userService.GetUser(userLogin);
            if (user == null)
            {
                return Unauthorized();
            }
            var token = tokenGenerator.Generate(user);
            return Ok(token);
        }
    }
}
