using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorld.Controllers;

[ApiController]
[Route("/hello")]
public class HelloController: ControllerBase
{
    [HttpGet]
    [Authorize]
    public string GetHello()
    {
        return "Hello, World!";
    }
}
