using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Blog.Data.Models.Db
{
    public class User : IdentityUser
    {
        /// <summary>
        /// Модель пользователя
        /// </summary>
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        [Required (ErrorMessage = "Не задано изображение")]
        [Url (ErrorMessage = "Некорректный адрес")]
        public string ImageUrl { get; set; }
        public List<Role> Roles { get; set; }
    }
}
