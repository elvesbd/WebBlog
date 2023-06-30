using Microsoft.AspNetCore.Mvc;
using WebBlog.Data;

namespace WebBlog.Controllers
{
    public class CategoryController : ControllerBase
    {
        [HttpGet("categories")]
        public IActionResult Get([FromServices] BlogDataContext ctx)
        {
            var categories = ctx.Categories.ToList();
            return Ok(categories);
        }
    }
}