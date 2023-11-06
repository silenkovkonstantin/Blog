using Microsoft.AspNetCore.Identity;

namespace Blog.Models.Db
{
    public class User : IdentityUser
    {
        /// <summary>
        /// Модель пользователя
        /// </summary>
        public string ImageUrl { get; set; }
        public Role Role { get; set; }

        public User()
        {
            Role.Id = 1;
            Role.Name = "Пользователь";
        }
    }
}
