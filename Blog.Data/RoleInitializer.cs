using Blog.Data.Models.Db;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            string adminEmail = "silenkov@gmail.com";
            string adminName = "silenkov";
            string password = "12345";

            if (await roleManager.FindByNameAsync("Пользователь") == null)
            {
                await roleManager.CreateAsync(new Role("Пользователь", "Стандартная роль приложения, присваиваемая всем пользователям по умолчанию"));
            }

            if (await roleManager.FindByNameAsync("Администратор") == null)
            {
                await roleManager.CreateAsync(new Role("Администратор", "Данная роль позволяет выполнять управление, редактирование, удаление ролей и пользователей в приложении"));
            }

            if (await roleManager.FindByNameAsync("Модератор") == null)
            {
                await roleManager.CreateAsync(new Role("Модератор", "Данная роль позволяет выполнять редактирование, удаление комментариев, тегов и статей в приложении"));
            }

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                User admin = new User
                {
                    Email = adminEmail,
                    UserName = adminName,
                    FirstName = "Константин",
                    LastName = "Силенков",
                };

                var result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Администратор");
                    await userManager.AddToRoleAsync(admin, "Пользователь");
                }
            }
        }
    }
}
