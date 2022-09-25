using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorld.Controllers
{
    [ApiController]
    [Route("/hello")]
    public class HelloController: ControllerBase
    {
        [HttpGet]
        public string GetHello()
        {
            return "Hello, World!";
        }

        [HttpGet]
        [Route("secured")]
        [Authorize]
        public string GetHelloSecured()
        {
            return "Hello, World!";
        }
    }
}
