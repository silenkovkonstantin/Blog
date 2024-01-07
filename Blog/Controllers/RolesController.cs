using AutoMapper;
using Blog.Data.Models.Db;
using Blog.Data.Repository;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class RolesController : Controller
    {
        private IMapper _mapper;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RolesController> _logger;

        public RolesController(IMapper mapper, RoleManager<Role> roleManager, UserManager<User> userManager, ILogger<RolesController> logger)
        {
            _mapper = mapper;
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        [Authorize(Roles = "Администратор")]
        [Route("Roles")]
        [HttpGet]
        public IActionResult Roles() => View(_roleManager.Roles);

        [Route("NewRole")]
        [HttpGet]
        public IActionResult NewRole()
        {
            return View("NewRole");
        }

        [Authorize(Roles = "Администратор")]
        [Route("NewRole")]
        [HttpPost]
        public async Task<IActionResult> NewRole(CreateRoleViewModel rolevm)
        {
            if (ModelState.IsValid)
            {
                if (_roleManager.Roles.Select(r => r.Name).Contains(rolevm.Name))
                {
                    return RedirectToAction("NewRole");
                }

                var role = _mapper.Map<Role>(rolevm);
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"Добавлена новая роль {role.Id}");
                    return View("Roles", _roleManager.Roles);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View("NewRole", rolevm);
        }

        [Authorize(Roles = "Администратор")]
        [Route("RoleEdit")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var editmodel = _mapper.Map<RoleViewModel>(role);

            return View("RoleEdit", editmodel);
        }

        [Authorize(Roles = "Администратор")]
        [Route("RoleEdit")]
        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);
                role = _mapper.Map<RoleViewModel, Role>(model, role);
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"Изменена роль {role.Id}");
                    return RedirectToAction("Roles", "Roles");
                }
                else
                {
                    return RedirectToAction("RoleEdit", "Roles");
                }
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
                return View("RoleEdit", model);
            }
        }

        [Authorize(Roles = "Администратор")]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            await _roleManager.DeleteAsync(role);
            _logger.LogInformation($"Удалена роль {role.Id}");
            var roles = _roleManager.Roles;

            return View("Roles", roles);
        }
    }
}
