using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBlog.Data;
using WebBlog.Models;
using WebBlog.ViewModels;

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
                return Ok(categories);
            }
            catch (Exception _)
            {
                return StatusCode(500, "01XE1 - Falha interna do servidor!");
            }
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id, [FromServices] BlogDataContext ctx)
        {
            try
            {
                var category = await ctx.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound("Category not found!");

                return Ok(category);
            }
            catch (Exception _)
            {
                return StatusCode(500, "02XE2 - Falha interna do servidor!");
            }
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync([FromBody] EditorCategoryViewModel model, [FromServices] BlogDataContext ctx)
        {
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
                return Created($"v1/categories/{category.Id}", category);
            }
            catch (DbUpdateException _)
            {
                return StatusCode(500, "03XE3 - Não foi possível incluir a categoria!");
            }
            catch (Exception _)
            {
                return StatusCode(500, "03XE4 - Falha interna do servidor!");
            }
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] EditorCategoryViewModel model, [FromServices] BlogDataContext ctx)
        {
            try
            {
                var category = await ctx.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound("Category not found!");

                category.Name = model.Name;
                category.Slug = model.Slug;

                ctx.Categories.Update(category);
                await ctx.SaveChangesAsync();
                return Ok(category);
            }
            catch (DbUpdateException _)
            {
                return StatusCode(500, "04XE5 - Não foi possível atualizar a categoria!");
            }
            catch (Exception _)
            {
                return StatusCode(500, "04XE6 - Falha interna do servidor!");
            }
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromServices] BlogDataContext ctx)
        {
            try
            {
                var category = await ctx.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound("Category not found!");

                ctx.Categories.Remove(category);
                await ctx.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException _)
            {
                return StatusCode(500, "05XE7 - Não foi possível excluir a categoria!");
            }
            catch (Exception _)
            {
                return StatusCode(500, "05XE7 - Falha interna do servidor!");
            }
        }
    }
}