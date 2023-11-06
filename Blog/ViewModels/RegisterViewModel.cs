using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Имя", Prompt = "Введите имя")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Поле Email обязтельно для заполнения")]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "example.com")]
        public string EmailReg { get; set; }

        [Required(ErrorMessage = "Поле Пароль обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Prompt = "Введите пароль")]
        [StringLength(100, ErrorMessage = "Поле {0} должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 5)]
        public string PasswordReg { get; set; }

        [Required(ErrorMessage = "Обязательно подтвердите пароль")]
        [Compare("PasswordReg", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля", Prompt = "Повторно введите пароль")]
        public string PasswordConfirm { get; set; }
    }
}
