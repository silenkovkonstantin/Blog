using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Имя", Prompt = "Введите имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле Фамилия обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Фамилия", Prompt = "Введите фамилию")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Поле Никнейм обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Никнейм", Prompt = "Введите никнейм")]
        public string UserName { get; set; }

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
