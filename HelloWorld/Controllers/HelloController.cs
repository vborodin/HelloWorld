using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorld.Controllers;

[ApiController]
[Route("/hello")]
public class HelloController: ControllerBase
{
    [HttpGet]
    [Authorize]
    public string Hello()
    {
        var givenName = this.HttpContext.User.Claims
            .Where(x => x.Type == ClaimTypes.GivenName)
            .Select(x => x.Value).FirstOrDefault() ?? "null";
        var surname = this.HttpContext.User.Claims
            .Where(x => x.Type == ClaimTypes.Surname)
            .Select(x => x.Value).FirstOrDefault() ?? "null";
        return $"Hello, {givenName} {surname}!";
    }
}
