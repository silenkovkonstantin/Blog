using AutoMapper;
using Blog.Data.Repository;
using Blog.Data.UoW;
using Blog.Models.Db;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class TagsController : Controller
    {
        private IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private IUnitOfWork _unitOfWork;

        public TagsController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "Администратор")]
        [Route("Tags")]
        [HttpPost]
        public async Task<IActionResult> Tags()
        {
            var repository = _unitOfWork.GetRepository<Tag>() as TagsRepository;
            var tags = await repository.GetAllAsync();
            var model = new TagsViewModel()
            {
                Tags = tags.OrderBy(x => x.Name).ToList(),
            };

            return View("Tags", model);
        }

        [Authorize(Roles = "Администратор")]
        [Route("NewTag")]
        [HttpPost]
        public async Task<IActionResult> NewTag(TagsViewModel tagsvm)
        {
            var currentuser = User;
            var user = await _userManager.GetUserAsync(currentuser);
            var repository = _unitOfWork.GetRepository<Tag>() as TagsRepository;
            
            var tag = new Tag()
            {
                Name = tagsvm.NewTag.Name,
            };

            await repository.CreateAsync(tag);
            var tags = await repository.GetAllAsync();

            var model = new TagsViewModel()
            {
                Tags = tags.OrderBy(x => x.Name).ToList(),
            };

            return View("Tags", model);
        }

        [Authorize(Roles = "Администратор")]
        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(int id, TagsViewModel tagsvm)
        {
            var currentuser = User;
            var user = await _userManager.GetUserAsync(currentuser);
            var repository = _unitOfWork.GetRepository<Tag>() as TagsRepository;

            var tag = await repository.GetAsync(id);
            tag.Name = tagsvm.NewTag.Name;

            await repository.UpdateAsync(tag);
            var tags = await repository.GetAllAsync();

            var model = new TagsViewModel()
            {
                Tags = tags.OrderBy(x => x.Name).ToList(),
            };

            return View("Tags", model);
        }

        [Authorize(Roles = "Администратор")]
        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id, TagsViewModel tagsvm)
        {
            var currentuser = User;
            var user = await _userManager.GetUserAsync(currentuser);
            var repository = _unitOfWork.GetRepository<Tag>() as TagsRepository;

            var tag = await repository.GetAsync(id);
            await repository.DeleteAsync(tag);
            var tags = await repository.GetAllAsync();

            var model = new TagsViewModel()
            {
                Tags = tags.OrderBy(x => x.Name).ToList(),
            };

            return View("Tags", model);
        }

        private async Task<List<Tag>> GetAllTags()
        {
            var repository = _unitOfWork.GetRepository<Tag>() as TagsRepository;
            var comments = await repository.GetAllAsync();

            return comments.ToList();
        }

        private async Task<Tag> GetTag(int id)
        {
            var repository = _unitOfWork.GetRepository<Tag>() as TagsRepository;
            var tag = await repository.GetAsync(id);

            return tag;
        }
    }
}
