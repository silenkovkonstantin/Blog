using AutoMapper;
using Blog.Data.Models.Db;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Blog.Data;
using Blog.Data.Repository;

namespace Blog.Controllers
{
    public class AccountController : Controller
    {
        private IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private IRepository<Post> _postsRepository;
        private IRepository<Comment> _commentRepository;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager,
            IMapper mapper, IRepository<Post> postsRepository, IRepository<Comment> commentRepository, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _postsRepository = postsRepository;
            _commentRepository = commentRepository;
            _logger = logger;
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

        //[Route("Login")]
        //[HttpGet]
        //public IActionResult Login()
        //{
        //    return View("Login");
        //}
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
            var editmodel = _mapper.Map<UserEditViewModel>(user);
            editmodel.Roles = _roleManager.Roles.Select(r => new RoleViewModel { Id = r.Id, Name = r.Name, Description = r.Description }).ToList();

            return View("Edit", editmodel);
        }

        [Authorize(Roles = "Администратор")]
        [Route("Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                user = _mapper.Map<UserEditViewModel, User>(model, user);
                var rolesList = model.Roles;
                await _userManager.AddToRolesAsync(user, model.Roles.Where(r => r.IsChecked == true).Select(r => r.Name));
                var result = await _userManager.UpdateAsync(user);

                //var roles = await _userManager.GetRolesAsync(user);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"Изменены данные пользователя {user.Id}");

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

        //[Route("Login")]
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
                    _logger.LogInformation($"Пользователь {user.Id} вошел в систему");

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

        [HttpGet]
        //public IActionResult Logout(string returnUrl = null)
        //{
        //    return RedirectToAction("Posts", "Posts");
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation($"Пользователь {HttpContext.User.Identity.Name} вышел из системы");
            return RedirectToAction("Posts", "Posts");
        }

        [Authorize(Roles = "Администратор")]
        [HttpGet]
        public async Task<IActionResult> User(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userPosts = await _postsRepository.GetAllByUserIdAsync(id);
            user.Posts = userPosts.ToList();
            var userComments = await _commentRepository.GetAllByUserIdAsync(id);
            user.Comments = userComments.ToList();

            return View("User", user);
        }
    }
}
