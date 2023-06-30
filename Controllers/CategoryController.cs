using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBlog.Data;
using WebBlog.Models;

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
                return NotFound("Category not found!");

            return Ok(category);
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync([FromBody] Category model, [FromServices] BlogDataContext ctx)
        {
            await ctx.Categories.AddAsync(model);
            await ctx.SaveChangesAsync();
            return Created($"v1/categories/{model.Id}", model);
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] Category model, [FromServices] BlogDataContext ctx)
        {
            var category = await ctx.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound("Category not found!");

            category.Name = model.Name;
            category.Slug = model.Slug;

            ctx.Categories.Update(category);
            await ctx.SaveChangesAsync();
            return Created($"v1/categories/{model.Id}", model);
        }
    }
}