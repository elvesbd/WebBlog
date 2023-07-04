using Microsoft.AspNetCore.Mvc;
using WebBlog.Data;
using WebBlog.Services;
using WebBlog.ViewModels;
using WebBlog.Extensions;
using WebBlog.Models;
using SecureIdentity.Password;
using Microsoft.EntityFrameworkCore;

namespace WebBlog.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly BlogDataContext _blogDataContext;
    public AccountController(TokenService tokenService, BlogDataContext blogDataContext)
    {
        _tokenService = tokenService;
        _blogDataContext = blogDataContext;
    }

    [HttpPost("v1/accounts")]
    public async Task<IActionResult> Post([FromBody] RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = new User
        {
            Name = model.Name,
            Email = model.Email,
            Slug = model.Email.Replace("@", "-").Replace(".", "-"),
        };
        var password = PasswordGenerator.Generate(25);
        user.PasswordHash = PasswordHasher.Hash(password);

        try
        {
            await _blogDataContext.Users.AddAsync(user);
            await _blogDataContext.SaveChangesAsync();

            return Ok(new ResultViewModel<dynamic>(new
            {
                user = user.Email,
                password
            }));
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("01XE1 - Email is already in use!"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("01XE2 - Internal server error!"));
        }
    }

    [HttpPost("v1/accounts/login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = await _blogDataContext.Users
            .AsNoTracking()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Email == model.Email);

        if (user == null)
            return StatusCode(401, new ResultViewModel<string>("01XE3 - Invalid credentials!"));

        var isPasswordValid = PasswordHasher.Verify(user.PasswordHash, model.Password);
        if (!isPasswordValid)
            return StatusCode(401, new ResultViewModel<string>("01XE4 - Invalid credentials!"));

        try
        {
            var token = _tokenService.GenerateToken(user);
            return Ok(new ResultViewModel<string>(token, null));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("01XE5 - Internal server error!"));
        }
    }
}