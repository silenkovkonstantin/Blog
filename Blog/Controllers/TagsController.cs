using AutoMapper;
using Blog.Data.Repository;
using Blog.Data.UoW;
using Blog.Data.Models.Db;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Blog.Controllers
{
    public class TagsController : Controller
    {
        private IMapper _mapper;
        private IRepository<Tag> _tagsRepository;

        public TagsController(IMapper mapper, IRepository<Tag> tagsRepository)
        {
            _mapper = mapper;
            _tagsRepository = tagsRepository;
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
            var allTags = await GetAllTagsAsync();
            
            if (allTags.Select(t => t.Name).Contains(tag.Name))
            {
                return RedirectToAction("NewTag");
            }

            await _tagsRepository.CreateAsync(tag);
            var tags = await GetAllTagsAsync();

            return View("Tags", tags);
        }

        [Route("TagEdit")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var tag = await GetTagAsync(id);
            var tagvm = _mapper.Map<TagViewModel>(tag);
            return View("TagEdit", tagvm);
        }

        [Authorize(Roles = "Администратор")]
        [Route("TagEdit")]
        [HttpPost]
        public async Task<IActionResult> Edit(TagViewModel tagvm)
        {
            var tag = await _tagsRepository.GetAsync(tagvm.Id);
            tag = _mapper.Map<TagViewModel, Tag>(tagvm, tag);
            await _tagsRepository.UpdateAsync(tag);
            var tags = await GetAllTagsAsync();

            return View("Tags", tags);
        }

        [Authorize(Roles = "Администратор")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var tag = await GetTagAsync(id);
            await _tagsRepository.DeleteAsync(tag);
            var tags = await GetAllTagsAsync();

            return View("Tags", tags);
        }

        private async Task<List<Tag>> GetAllTagsAsync()
        {
            var tags = await _tagsRepository.GetAllAsync();

            return tags.ToList();
        }

        private async Task<Tag> GetTagAsync(int id)
        {
            var tag = await _tagsRepository.GetAsync(id);

            return tag;
        }
    }
}
