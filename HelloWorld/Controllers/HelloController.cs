using Microsoft.AspNetCore.Mvc;

namespace HelloWorld.Controllers
{
    [ApiController]
    [Route("/hello")]
    public class HelloController
    {
        [HttpGet]
        public string GetHello()
        {
            return "Hello, World!";
        }
    }
}
