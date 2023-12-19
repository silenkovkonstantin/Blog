using Microsoft.AspNetCore.Identity;

namespace Blog.Data.Models.Db
{
    public class Role : IdentityRole
    {
        /// <summary>
        /// Модель пользовательской роли
        /// </summary>
        public string Description { get; set; }
    }
}
