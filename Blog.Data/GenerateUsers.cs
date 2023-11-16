﻿using Blog.Data.Models.Db;
using Microsoft.AspNetCore.Identity;

namespace Blog.Data
{
    public class GenetateUsers
    {
        public readonly string[] names = new string[] { "patrickbateman", "ryangosling", "jasonstatham" };
        public readonly string[] images = new string[] { "https://i.pinimg.com/564x/34/63/34/346334640dd06e074b5b37c1e1263931.jpg",
            "https://i.pinimg.com/564x/6f/9e/00/6f9e00506669102bd4dd6f98a966e3cf.jpg",
            "https://i.pinimg.com/originals/5c/9c/36/5c9c363f068fd9808161e711257b0946.jpg" };
        public readonly string password = "123";
        public readonly string[] roles = new string[] { "Администратор", "Модератор", "Пользователь" };

        public List<User> Populate()
        {
            var users = new List<User>();

            for (int i = 0; i < names.Length; i++)
            {
                var role = new Role
                {
                    Id = i.ToString(),
                    Name = roles[i]
                };

                var user = new User()
                {
                    Email = names[i] + "@gmail.com",
                    ImageUrl = images[i],
                    UserName = names[i],
                    Roles = new List<Role> { role },
                    // Создание хэш-пароля и его верификацию допишу потом
                    //PasswordHash
                };

                users.Add(user);
            }

            return users;
        }
    }
}