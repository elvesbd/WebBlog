using Microsoft.AspNetCore.Mvc;
using WebBlog.Attributes;

namespace WebBlog.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : Controller
    {
        [ApiKey]
        [HttpGet("")]
        public IActionResult Get() => Ok();
    }
}