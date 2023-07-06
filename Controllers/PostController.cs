using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBlog.Data;
using WebBlog.Models;
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
            return StatusCode(500, new ResultViewModel<List<Post>>("04XE1 - Internal server failure!"));
        }
    }

    [HttpGet("v1/posts/{id:int}")]
    public async Task<IActionResult> DetailsAsync([FromRoute] int id)
    {
        try
        {
            var post = await _blogDataContext.Posts
                .AsNoTracking()
                .Include(p => p.Author)
                .ThenInclude(p => p.Roles)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
                return NotFound(new ResultViewModel<Post>("Post not found!"));

            return Ok(new ResultViewModel<Post>(post));
        }
        catch (System.Exception)
        {
            return StatusCode(500, new ResultViewModel<string>("04XE2 - Internal server error!"));
        }
    }

    [HttpGet("v1/posts/category/{category}")]
    public async Task<IActionResult> GetByCategoryAsync(
        [FromRoute] string category,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 25
    )
    {
        try
        {
            var count = await _blogDataContext.Posts.AsNoTracking().CountAsync();
            var posts = await _blogDataContext.Posts
                .AsNoTracking()
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Where(p => p.Category.Slug == category)
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
            return StatusCode(500, new ResultViewModel<List<Post>>("04XE3 - Internal server error!")); ;
        }
    }
}