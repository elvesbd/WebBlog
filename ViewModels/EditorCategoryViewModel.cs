using System.ComponentModel.DataAnnotations;

namespace WebBlog.ViewModels
{
    public class EditorCategoryViewModel
    {
        [Required(ErrorMessage = "O campo name é obrigatório")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "O campo name deve conter no mínimo 3 caracteres e no máximo 40 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo slug é obrigatório")]
        public string Slug { get; set; }

    }
}