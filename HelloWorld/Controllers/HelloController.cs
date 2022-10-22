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
        return $"Hello, {name}!";
    }
}
