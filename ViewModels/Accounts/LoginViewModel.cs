using System.ComponentModel.DataAnnotations;

namespace WebBlog.ViewModels.Accounts;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email must be informed!")]
    [EmailAddress(ErrorMessage = "Invalid email!")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password must be informed!")]
    public string Password { get; set; }
}