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
        // Навигационное свойство
        public List<Post> Posts { get; set; } = new List<Post>();
        // Навигационное свойство
        public List<Comment> Comments { get; set;} = new List<Comment>();
    }
}
