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
        [HttpPost]
        public async Task<IActionResult> Tags()
        {
            var repository = _unitOfWork.GetRepository<Tag>() as TagsRepository;
            var tags = await GetAllTagsAsync();
            var model = _mapper.Map<IEnumerable<Tag>, TagsViewModel>(tags);

            return View("Tags", model);
        }

        [Authorize(Roles = "Администратор")]
        [Route("NewTag")]
        [HttpPost]
        public async Task<IActionResult> NewTag(TagsViewModel tagsvm)
        {
            var repository = _unitOfWork.GetRepository<Tag>() as TagsRepository;
            var tag = _mapper.Map<TagsViewModel, Tag>(tagsvm);
            await repository.CreateAsync(tag);
            var tags = await GetAllTagsAsync();
            var model = _mapper.Map<IEnumerable<Tag>, TagsViewModel>(tags);

            return View("Tags", model);
        }

        [Authorize(Roles = "Администратор")]
        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(TagsViewModel tagsvm)
        {
            var repository = _unitOfWork.GetRepository<Tag>() as TagsRepository;
            var tag = _mapper.Map<TagsViewModel, Tag>(tagsvm);
            await repository.UpdateAsync(tag);
            var tags = await GetAllTagsAsync();
            var model = _mapper.Map<IEnumerable<Tag>, TagsViewModel>(tags);

            return View("Tags", model);
        }

        [Authorize(Roles = "Администратор")]
        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id, TagsViewModel tagsvm)
        {
            var repository = _unitOfWork.GetRepository<Tag>() as TagsRepository;
            var tag = await GetTagAsync(id);
            await repository.DeleteAsync(tag);
            var tags = await GetAllTagsAsync();
            var model = _mapper.Map<IEnumerable<Tag>, TagsViewModel>(tags);

            return View("Tags", model);
        }

        private async Task<List<Tag>> GetAllTagsAsync()
        {
            var repository = _unitOfWork.GetRepository<Tag>() as TagsRepository;
            var comments = await repository.GetAllAsync();

            return comments.ToList();
        }

        private async Task<Tag> GetTagAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Tag>() as TagsRepository;
            var tag = await repository.GetAsync(id);

            return tag;
        }
    }
}
