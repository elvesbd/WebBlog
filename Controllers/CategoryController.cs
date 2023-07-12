using WebBlog.Data;
using WebBlog.Models;
using WebBlog.ViewModels.Categories;
using WebBlog.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace WebBlog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        public CategoryController(IMemoryCache cache)
        {
            _cache = cache;
        }

        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync([FromServices] BlogDataContext ctx)
        {
            try
            {
                var categories = _cache.GetOrCreate("CategoriesCache", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                    return GetCategories(ctx);
                });

                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("02XE1 - Internal server failure!"));
            }
        }
        private List<Category> GetCategories(BlogDataContext ctx)
        {
            return ctx.Categories.ToList();
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
            catch (Exception)
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
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("02XE3 - Unable to add category!"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Category>("02XE4 - Internal server failure!"));
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
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("02XE5 - Unable to update category"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Category>("02XE6 - Internal server failure!"));
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
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("02XE7 - Não foi possível excluir a categoria!"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Category>("02XE8 - Internal server failure!"));
            }
        }
    }
}