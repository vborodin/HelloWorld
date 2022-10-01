using AuthenticationService.Models;
using AuthenticationService.Repository.Model;
using AuthenticationService.Services;
using AuthenticationService.TokenGenerator;

using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers;

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
    public IActionResult Login([FromBody] UserLogin userLogin, [FromQuery] string audience, [FromQuery] int expirationPeriodMinutes = 15)
    {
        var user = this.userService.GetUser(userLogin);
        if (user == null)
        {
            return Unauthorized();
        }
        var token = this.tokenGenerator.Generate(user, audience, expirationPeriodMinutes);
        return Ok(token);
    }
}
