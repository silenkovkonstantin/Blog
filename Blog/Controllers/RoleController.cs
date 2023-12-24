using AutoMapper;
using Blog.Data.Models.Db;
using Blog.Data.Repository;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class RoleController : Controller
    {
        private IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleController(IMapper mapper, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _mapper = mapper;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [Authorize(Roles = "Администратор")]
        [Route("Roles")]
        [HttpGet]
        public IActionResult Roles() => View(_roleManager.Roles.ToList());

        //[Authorize(Roles = "Администратор")]
        //[Route("Edit")]
        //[HttpGet]
        //public async Task<IActionResult> Edit()
        //{
        //    var allRoles = _roleManager.Roles.ToList();
        //    var editmodel = _mapper.Map<RoleEditViewModel>(result);

        //    return View("Edit", editmodel);
        //}
    }
}
