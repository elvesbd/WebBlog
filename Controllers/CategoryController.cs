using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBlog.Data;

namespace WebBlog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync([FromServices] BlogDataContext ctx)
        {
            var categories = await ctx.Categories.ToListAsync();
            return Ok(categories);
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id, [FromServices] BlogDataContext ctx)
        {
            var category = await ctx.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }
    }
}