﻿using AuthenticationService.Controllers.Dtos;
using AuthenticationService.Services;
using AuthenticationService.Services.Model;
using AuthenticationService.TokenGenerator;

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
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] UserLoginDto userLogin, [FromQuery] string audience, [FromQuery] int expirationPeriodMinutes = 15)
    {
        if (!IsAudienceValid(audience))
        {
            return BadRequest($"{nameof(audience)} must not be empty");
        }
        if (!IsExpiratonPeriodValid(expirationPeriodMinutes))
        {
            return BadRequest($"{nameof(expirationPeriodMinutes)} must be positive");
        }
        var user = this.userService.GetUser(userLogin.Username, userLogin.Password);
        if (user == null)
        {
            return Unauthorized();
        }
        var token = this.tokenGenerator.Generate(user, audience, expirationPeriodMinutes);
        return Ok(token);
    }

    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Register([FromBody] UserRegistrationDto userRegistrationDto)
    {
        this.userService.CreateUser(
            username: userRegistrationDto.Username,
            password: userRegistrationDto.Password,
            email: userRegistrationDto.Email,
            givenName: userRegistrationDto.GivenName,
            surname: userRegistrationDto.Surname);
        return Ok();
    }

    private bool IsExpiratonPeriodValid(int expirationPeriodMinutes)
    {
        return expirationPeriodMinutes > 0;
    }

    private bool IsAudienceValid(string audience)
    {
        return !string.IsNullOrEmpty(audience);
    }
}
