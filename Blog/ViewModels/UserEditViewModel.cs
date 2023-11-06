using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class UserEditViewModel
    {
        [Required]
        [Display(Name = "Идентификатор пользователя")]
        public string UserId { get; set; }

        [EmailAddress]
        [Display(Name = "Email", Prompt = "example.com")]
        public string Email { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Фото", Prompt = "Ссылка на изображение")]
        public string ImageUrl { get; set; }
    }
}
