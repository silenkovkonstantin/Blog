using AutoMapper;
using Blog.Data.UoW;
using Blog.Extensions;
using Blog.Data.Models.Db;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Blog.Data;

namespace Blog.Controllers
{
    public class AccountController : Controller
    {
        private IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private IUnitOfWork _unitOfWork;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        
        [Route("Generate")]
        [HttpGet]
        public async Task<IActionResult> Generate()
        {

            var usergen = new GenetateUsers();
            var userlist = usergen.Populate();

            foreach (var user in userlist)
            {
                var result = await _userManager.CreateAsync(user);

                if (!result.Succeeded)
                    continue;
            }

            return RedirectToAction("Index", "Home");
        }

        [Route("RoleGenerate")]
        [HttpGet]
        public async Task<IActionResult> RoleGenerate()
        {
            await RoleInitializer.InitializeAsync(_userManager, _roleManager);

            return RedirectToAction("Index", "Home");
        }

        [Route("Login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [Route("Users")]
        [HttpGet]
        public IActionResult Users()
        {
            var users = _userManager.Users;

            return View("Users", users);
        }

        [Authorize(Roles = "Администратор")]
        [Route("Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var editmodel = _mapper.Map<UserViewModel>(user);
            editmodel.Roles = _roleManager.Roles.Select(r => new RoleViewModel { Name = r.Name }).ToList();

            return View("Edit", editmodel);
        }

        [Authorize(Roles = "Администратор")]
        [Route("Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var user = await _userManager.FindByEmailAsync(model.Email);
                //user.Convert(model);

                var user = _mapper.Map<User>(model);

                var rolesList = model.Roles;

                await _userManager.AddToRolesAsync(user, model.Roles.Where(r => r.IsChecked == true).Select(r => r.Name));
                var result = await _userManager.UpdateAsync(user);

                var roles = await _userManager.GetRolesAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Users", "Account");
                }
                else
                {
                    return RedirectToAction("Edit", "Account");
                }
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
                return View("Edit", model);
            }
        }

        [Route("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    // Проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и(или) пароль");
                }
            }

            return View(model);
        }

        [Route("Logout")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


    }
}
