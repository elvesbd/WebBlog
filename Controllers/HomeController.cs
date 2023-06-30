using Microsoft.AspNetCore.Mvc;

namespace WebBlog.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}