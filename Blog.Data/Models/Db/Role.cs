using Microsoft.AspNetCore.Identity;

namespace Blog.Data.Models.Db
{
    public class Role : IdentityRole
    {
        /// <summary>
        /// Модель пользовательской роли
        /// </summary>
        public string Description { get; set; }

        public Role() { }
        public Role(string roleName, string description)
        {
            Name = roleName;
            Description = description;
        }
    }
}
