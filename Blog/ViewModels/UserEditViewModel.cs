using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class UserEditViewModel : UserViewModel
    {
        [Required]
        [Display(Name = "Идентификатор пользователя")]
        public string Id { get; set; }
    }
}
