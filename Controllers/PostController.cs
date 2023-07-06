using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBlog.Data;
using WebBlog.ViewModels.Categories;
using WebBlog.ViewModels.Posts;

namespace WebBlog.Controllers;

[ApiController]
public class PostController : ControllerBase
{
    private readonly BlogDataContext _blogDataContext;
    public PostController(BlogDataContext blogDataContext)
    {
        _blogDataContext = blogDataContext;
    }

    [HttpGet("v1/posts")]
    public async Task<IActionResult> GetAsync([FromQuery] int page = 0, [FromQuery] int pageSize = 25)
    {
        try
        {
            var count = await _blogDataContext.Posts.AsNoTracking().CountAsync();
            var posts = await _blogDataContext.Posts
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Author)
                .Select(x => new ListPostViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Slug = x.Slug,
                    LasUpdateDate = x.LastUpdateDate,
                    Category = x.Category.Name,
                    Author = $"{x.Author.Name} ({x.Author.Email})",
                })
                .Skip(page * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.LasUpdateDate)
                .ToListAsync();
            return Ok(new ResultViewModel<dynamic>(new
            {
                total = count,
                page,
                pageSize,
                posts
            }));
        }
        catch (System.Exception)
        {

            throw;
        }
    }
}