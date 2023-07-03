using WebBlog.Data;
using WebBlog.Models;
using WebBlog.ViewModels;
using WebBlog.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebBlog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync([FromServices] BlogDataContext ctx)
        {
            try
            {
                var categories = await ctx.Categories.ToListAsync();
                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch (Exception _)
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("01XE1 - Internal server failure!"));
            }
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id, [FromServices] BlogDataContext ctx)
        {
            try
            {
                var category = await ctx.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Category not found!"));

                return Ok(category);
            }
            catch (Exception _)
            {
                return StatusCode(500, new ResultViewModel<Category>("02XE2 - Internal server failure!"));
            }
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync([FromBody] EditorCategoryViewModel model, [FromServices] BlogDataContext ctx)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            try
            {
                var category = new Category
                {
                    Id = 0,
                    Name = model.Name,
                    Slug = model.Slug
                };
                await ctx.Categories.AddAsync(category);
                await ctx.SaveChangesAsync();
                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException _)
            {
                return StatusCode(500, new ResultViewModel<Category>("03XE3 - Unable to add category!"));
            }
            catch (Exception _)
            {
                return StatusCode(500, new ResultViewModel<Category>("03XE4 - Internal server failure!"));
            }
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] EditorCategoryViewModel model, [FromServices] BlogDataContext ctx)
        {
            try
            {
                var category = await ctx.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Category not found!"));

                category.Name = model.Name;
                category.Slug = model.Slug;

                ctx.Categories.Update(category);
                await ctx.SaveChangesAsync();
                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException _)
            {
                return StatusCode(500, new ResultViewModel<Category>("04XE5 - Unable to update category"));
            }
            catch (Exception _)
            {
                return StatusCode(500, new ResultViewModel<Category>("04XE6 - Internal server failure!"));
            }
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromServices] BlogDataContext ctx)
        {
            try
            {
                var category = await ctx.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Category not found!"));

                ctx.Categories.Remove(category);
                await ctx.SaveChangesAsync();
                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException _)
            {
                return StatusCode(500, new ResultViewModel<Category>("05XE7 - Não foi possível excluir a categoria!"));
            }
            catch (Exception _)
            {
                return StatusCode(500, new ResultViewModel<Category>("05XE7 - Internal server failure!"));
            }
        }
    }
}