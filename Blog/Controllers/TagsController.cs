using AutoMapper;
using Blog.Data.Repository;
using Blog.Data.UoW;
using Blog.Data.Models.Db;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class TagsController : Controller
    {
        private IMapper _mapper;
        private IUnitOfWork _unitOfWork;

        public TagsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "Администратор")]
        [Route("Tags")]
        [HttpGet]
        public async Task<IActionResult> Tags()
        {
            var tags = await GetAllTagsAsync();

            return View("Tags", tags);
        }

        [Route("NewTag")]
        [HttpGet]
        public IActionResult NewTag()
        {
            return View("NewTag");
        }

        [Authorize(Roles = "Администратор")]
        [Route("NewTag")]
        [HttpPost]
        public async Task<IActionResult> NewTag(Tag tag)
        {
            var repository = _unitOfWork.GetRepository<Tag>() as TagsRepository;
            var allTags = await GetAllTagsAsync();
            
            if (allTags.Select(t => t.Name).Contains(tag.Name))
            {
                return RedirectToAction("NewTag");
            }

            await repository.CreateAsync(tag);
            var tags = await GetAllTagsAsync();

            return View("Tags", tags);
        }

        [Route("TagEdit")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var tag = await GetTagAsync(id);
            return View("TagEdit", tag);
        }

        [Authorize(Roles = "Администратор")]
        [Route("TagEdit")]
        [HttpPost]
        public async Task<IActionResult> Edit(Tag tag)
        {
            var repository = _unitOfWork.GetRepository<Tag>() as TagsRepository;
            await repository.UpdateAsync(tag);
            var tags = await GetAllTagsAsync();

            return View("Tags", tags);
        }

        [Authorize(Roles = "Администратор")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var repository = _unitOfWork.GetRepository<Tag>() as TagsRepository;
            var tag = await GetTagAsync(id);
            await repository.DeleteAsync(tag);
            var tags = await GetAllTagsAsync();

            return View("Tags", tags);
        }

        private async Task<List<Tag>> GetAllTagsAsync()
        {
            var repository = _unitOfWork.GetRepository<Tag>() as TagsRepository;
            var tags = await repository.GetAllAsync();

            return tags.ToList();
        }

        private async Task<Tag> GetTagAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Tag>() as TagsRepository;
            var tag = await repository.GetAsync(id);

            return tag;
        }
    }
}
