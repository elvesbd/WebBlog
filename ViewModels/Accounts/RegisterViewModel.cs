using System.ComponentModel.DataAnnotations;

namespace WebBlog.ViewModels.Accounts;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Name is required!")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required!")]
    [EmailAddress(ErrorMessage = "Email is invalid!")]
    public string Email { get; set; } = string.Empty;
}