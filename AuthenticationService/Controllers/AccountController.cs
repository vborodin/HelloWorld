using AuthenticationService.Controllers.Dtos;
using AuthenticationService.Services;
using AuthenticationService.Services.Exceptions;
using AuthenticationService.Services.Model;
using AuthenticationService.Services.TokenGenerator;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IUserService userService;
    private readonly ITokenGenerator<UserModel> tokenGenerator;

    public AccountController(IUserService userService, ITokenGenerator<UserModel> tokenGenerator)
    {
        this.userService = userService;
        this.tokenGenerator = tokenGenerator;
    }

    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
    public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto userLogin, [FromQuery] string audience, [FromQuery] int expirationPeriodMinutes = 15)
    {
        if (!IsExpiratonPeriodValid(expirationPeriodMinutes))
        {
            return BadRequest($"{nameof(expirationPeriodMinutes)} must be positive");
        }
        var user = await this.userService.GetUserAsync(userLogin.Username, userLogin.Password);
        if (user == null)
        {
            return Unauthorized("Invalid username or password");
        }
        var token = this.tokenGenerator.Generate(user, audience, expirationPeriodMinutes);
        return Ok(token);
    }

    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Register([FromBody] UserRegistrationDto userRegistrationDto)
    {
        await this.userService.CreateUserAsync(
            username: userRegistrationDto.Username,
            password: userRegistrationDto.Password,
            email: userRegistrationDto.Email,
            givenName: userRegistrationDto.GivenName,
            surname: userRegistrationDto.Surname);
        return Ok();
    }

    [HttpPost("Role")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<ActionResult> SetRole([FromBody] SetRoleDto setRoleDto)
    {
        try
        {
            await this.userService.SetRoleAsync(setRoleDto.Username, setRoleDto.Role);
        }
        catch (RoleAssignmentException e)
        {
            return BadRequest(e.Message);
        }
        return Ok();
    }

    private bool IsExpiratonPeriodValid(int expirationPeriodMinutes)
    {
        return expirationPeriodMinutes > 0;
    }
}
