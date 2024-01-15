using AutoMapper;
using Blog.Data.Repository;
using Blog.Data.Models.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlogAPI.Contracts.Models.Tags;
using NuGet.Protocol.Plugins;
using static BlogAPI.Contracts.Models.Tags.GetTagsResponse;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class TagsController : ControllerBase
    {
        private IMapper _mapper;
        private IRepository<Tag> _tagsRepository;

        public TagsController(IMapper mapper, IRepository<Tag> tagsRepository)
        {
            _mapper = mapper;
            _tagsRepository = tagsRepository;
        }

        /// <summary>
        /// Просмотр списка тегов
        /// </summary>
        /// <remarks>
        /// GET /Tags
        /// </remarks>
        /// <returns>Returns All Tags</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "Модератор, Пользователь")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Tags()
        {
            var tags = await GetAllTagsAsync();
            var response = new GetTagsResponse
            {
                TagAmount = tags.Count,
                Tags = _mapper.Map<List<Tag>, List<TagView>>(tags)
            };

            return StatusCode(200, response);
        }

        /// <summary>
        /// Добавление нового тега
        /// </summary>
        /// <remarks>
        /// POST /Tags/Add
        /// </remarks>
        /// <param name="request">AddTagRequest object</param>
        /// <returns>Добавляет новый тег</returns>
        /// <response code="201">Create a tag in the system</response>
        /// <response code="400">Unable to create the tag due to validation error</response>
        [HttpPost]
        [Route("[action]")]
        [Authorize(Roles = "Пользователь")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Add([FromBody] AddTagRequest request)
        {
            var allTags = await GetAllTagsAsync();
            
            if (allTags.Select(t => t.Name).Contains(request.Name))
            {
                return StatusCode(409, $"Ошибка: Тег {request.Name} уже существует.");
            }

            var tag = _mapper.Map<Tag>(request);
            await _tagsRepository.CreateAsync(tag);

            return StatusCode(201, $"Тег {request.Name} добавлена!");
        }

        /// <summary>
        /// Редактирование тега по id
        /// </summary>
        /// <remarks>
        /// PATCH /Tags/Edit/id
        /// </remarks>
        /// <param name="id">Tag id (int)</param>      
        /// <param name="request">EditTagRequest object</param>
        /// <returns>Возвращает обновленное название тега</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpPatch]
        [Route("[action]/{id}")]
        [Authorize(Roles = "Модератор")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] EditTagRequest request)
        {
            var tag = await _tagsRepository.GetAsync(id);
            if (tag == null)
            {
                return StatusCode(400, $"Ошибка: Тег с идентификатором {id} не существует.");
            }
                
            tag = _mapper.Map<EditTagRequest, Tag>(request, tag);
            await _tagsRepository.UpdateAsync(tag);

            return StatusCode(200, $"Тег с идентификатором {id} обновлен!");
        }

        /// <summary>
        /// Удаление тега по id
        /// </summary>
        /// <remarks>
        /// DELETE /Tags/Delete/id
        /// </remarks>
        /// <param name="id">Tag id (int)</param>      
        /// <returns>Возвращает: NoContent</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpDelete]
        [Route("[action]/{id}")]
        [Authorize(Roles = "Модератор")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var tag = await GetTagAsync(id);
            if (tag == null)
            {
                return StatusCode(400, $"Ошибка: Тег с идентификатором {id} не существует!");
            }
                
            await _tagsRepository.DeleteAsync(tag);

            return StatusCode(200, $"Тег: \"{tag.Name}\", с идентификатором: {tag.Id} удален!");
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
