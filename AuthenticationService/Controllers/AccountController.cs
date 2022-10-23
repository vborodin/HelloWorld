using AuthenticationService.Controllers.Dtos;
using AuthenticationService.Services;
using AuthenticationService.Services.Exceptions;
using AuthenticationService.Services.Model;
using AuthenticationService.Services.TokenGenerator;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using NUnit.Framework;

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
    public async Task<IActionResult> LoginAsync([FromBody] UserPasswordDto userLogin, [FromQuery] string audience, [FromQuery] int expirationPeriodMinutes = 15)
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
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<ActionResult> RegisterAsync([FromBody] UserPasswordDto userPasswordDto)
    {
        try
        {
            await this.userService.CreateUserAsync(
                username: userPasswordDto.Username,
                password: userPasswordDto.Password);
        }
        catch (RegistrationException e)
        {
            return BadRequest(e.Message);
        }
        
        return Ok();
    }

    [HttpPost("Role")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<ActionResult> AddRoleAsync([FromBody] SetRoleDto setRoleDto)
    {
        try
        {
            await this.userService.AddRoleAsync(setRoleDto.Username, setRoleDto.Role);
        }
        catch (RoleAssignmentException e)
        {
            return BadRequest(e.Message);
        }
        catch (RoleExistenceException e)
        {
            return BadRequest(e.Message);
        }
        return Ok();
    }

    [HttpDelete("Role")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult> RemoveRoleAsync()
    {
        throw new NotImplementedException();
    }

    private bool IsExpiratonPeriodValid(int expirationPeriodMinutes)
    {
        return expirationPeriodMinutes > 0;
    }
}
