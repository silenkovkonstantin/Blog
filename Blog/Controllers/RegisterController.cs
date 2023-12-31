﻿using AutoMapper;
using Blog.Data.Models.Db;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks.Dataflow;

namespace Blog.Controllers
{
    public class RegisterController : Controller
    {
        private IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager,
            ILogger<RegisterController> logger)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [Route("Register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);

                var result = await _userManager.CreateAsync(user, model.PasswordReg);
                if (result.Succeeded)
                {
                    // Установка куки
                    await _userManager.AddToRoleAsync(user, "Пользователь");
                    await _signInManager.SignInAsync(user, false);
                    _logger.LogInformation($"Зарегистрирован новый пользователь {user.Id}");

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View("Register", model);
        }
    }
}
