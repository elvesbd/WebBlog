using Microsoft.AspNetCore.Mvc;
using WebBlog.Data;

namespace WebBlog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public IActionResult Get([FromServices] BlogDataContext ctx)
        {
            var categories = ctx.Categories.ToList();
            return Ok(categories);
        }
    }
}