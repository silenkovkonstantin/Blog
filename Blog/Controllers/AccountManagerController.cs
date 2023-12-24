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
    public class AccountManagerController : Controller
    {
        private IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private IUnitOfWork _unitOfWork;

        public AccountManagerController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

        [Route("Login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        //[HttpGet]
        //public IActionResult Login(string returnUrl = null)
        //{
        //    return View(new LoginViewModel { ReturnUrl = returnUrl });
        //}

        [Authorize(Roles = "Администратор")]
        [Route("Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = User;
            var result = await _userManager.GetUserAsync(user);
            var editmodel = _mapper.Map<UserEditViewModel>(result);

            return View("Edit", editmodel);
        }

        [Authorize(Roles = "Администратор")]
        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                user.Convert(model);
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("MyPage", "AccountManager");
                }
                else
                {
                    return RedirectToAction("Edit", "AccountManager");
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
                //var user = _mapper.Map<User>(model);

                var user = await _userManager.FindByEmailAsync(model.Email);

                //var passTest = await _userManager.CheckPasswordAsync(user, model.Password);

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, "Администратор"),//user.Roles.OrderBy(r => r.Name).First().Name)
                        // Роли в списке сортируются в порядке: Администратор, Модератор, Пользователь.
                        // Первая в списке всегда с максимальными правами.
                    };

                    //claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email));
                    //claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, "Администратор"));
                    var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и(или) пароль");
                }
            }

            return RedirectToAction("Index", "Home");
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
