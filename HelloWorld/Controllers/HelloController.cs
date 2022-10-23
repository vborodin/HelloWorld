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
        var name = this.HttpContext.User.Claims
            .Where(x => x.Type == ClaimTypes.NameIdentifier)
            .Select(x => x.Value).FirstOrDefault() ?? "null";
        var roles = this.HttpContext.User.Claims
            .Where(x => x.Type == ClaimTypes.Role)
            .Aggregate("", (roles, claim) =>
            {
                var prev = string.IsNullOrWhiteSpace(roles) ? "" : $"{roles}, ";
                return $"{prev}{claim.Value}";
            });
        return $"Hello, {name}! Your roles: {roles}";
    }
}
