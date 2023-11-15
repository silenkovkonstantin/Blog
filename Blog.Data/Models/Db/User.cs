using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Blog.Data.Models.Db
{
    public class User : IdentityUser
    {
        /// <summary>
        /// Модель пользователя
        /// </summary>
        [Required (ErrorMessage = "Не задано изображение")]
        [Url (ErrorMessage = "Некорректный адрес")]
        public string ImageUrl { get; set; }
        public List<Role> Roles { get; set; }
    }
}
