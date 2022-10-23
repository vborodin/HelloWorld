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
    public async Task<IActionResult> LoginAsync([FromBody] UsernamePasswordDto usernamePasswordDto, [FromQuery] string audience, [FromQuery] int expirationPeriodMinutes = 15)
    {
        if (!IsExpiratonPeriodValid(expirationPeriodMinutes))
        {
            return BadRequest($"{nameof(expirationPeriodMinutes)} must be positive");
        }
        var user = await this.userService.GetUserAsync(usernamePasswordDto.Username, usernamePasswordDto.Password);
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
    public async Task<ActionResult> RegisterAsync([FromBody] UsernamePasswordDto usernamePasswordDto)
    {
        try
        {
            await this.userService.CreateUserAsync(
                username: usernamePasswordDto.Username,
                password: usernamePasswordDto.Password);
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
    public async Task<ActionResult> AddRoleAsync([FromBody] UsernameRoleDto usernameRoleDto)
    {
        try
        {
            await this.userService.AddRoleAsync(usernameRoleDto.Username, usernameRoleDto.Role);
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<ActionResult> RemoveRoleAsync([FromBody] UsernameRoleDto usernameRoleDto)
    {
        try
        {
            await this.userService.RemoveRoleAsync(usernameRoleDto.Username, usernameRoleDto.Role);
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
