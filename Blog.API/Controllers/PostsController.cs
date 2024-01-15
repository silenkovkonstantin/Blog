using AutoMapper;
using Blog.Data.Repository;
using Blog.Data.Models.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BlogAPI.Contracts.Models.Posts;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private IRepository<Post> _postsRepository;
        private IRepository<Tag> _tagsRepository;

        public PostsController(IMapper mapper, UserManager<User> userManager, IRepository<Post> postsRepository,
            IRepository<Tag> tagsRepository)
        {
            _mapper = mapper;
            _userManager = userManager;
            _postsRepository = postsRepository;
            _tagsRepository = tagsRepository;
        }

        /// <summary>
        /// Просмотр списка статей
        /// </summary>
        /// <remarks>
        /// GET /Posts
        /// </remarks>
        /// <returns>Returns All Posts</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpGet]
        [Route("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Posts()
        {
            var posts = await GetAllPostsAsync();
            var response = new GetPostsResponse
            {
                PostAmount = posts.Count,
                Posts = _mapper.Map<List<Post>, List<PostView>>(posts)
            };

            return StatusCode(200, response);
        }

        /// <summary>
        /// Просмотр статьи по названию
        /// </summary>
        /// <remarks>
        /// POST /Posts/Post/id
        /// </remarks>
        /// <param name="id">Post id (int)</param>
        /// <returns>Возвращает запрашивоемую статью</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpPost]
        [Route("[action]/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Post([FromRoute] int id)
        {
            var post = await _postsRepository.GetAsync(id);
            if (post == null)
            {
                return StatusCode(400, $"Ошибка: Статья с идентификатором: \"{id}\" не найдена. Проверьте корректность ввода!");
            }

            var response = _mapper.Map<PostView>(post);

            return StatusCode(200, response);
        }

        /// <summary>
        /// Добавление новой статьи
        /// </summary>
        /// <remarks>
        /// POST /Posts/Add
        /// </remarks>
        /// <param name="request">AddPostRequest object</param>
        /// <returns>Добавляет новую статью</returns>
        /// <response code="201">Success</response>
        /// <response code="400">If the user is unauthorized</response>
        [HttpPost]
        [Route("[action]")]
        [Authorize(Roles = "Пользователь")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Add([FromBody] AddPostRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return StatusCode(400, $"Ошибка: Пользователь \"{request.UserId}\" не зарегестрирован. Сначала пройдите регистрацию!");
            }
                
            var post = _mapper.Map<AddPostRequest, Post>(request);
            var allTags = await _tagsRepository.GetAllAsync();
            var tagsId = post.Tags.Select(t => t.Id);
            post.Tags = allTags.Where(t => tagsId.Contains(t.Id)).ToList();
            await _postsRepository.CreateAsync(post);

            return StatusCode(200, $"Статья: \"{request.Title}\", автора {user.Email}, добавлена!");
        }

        /// <summary>
        /// Редактирование статьи по id
        /// </summary>
        /// <remarks>
        /// PATCH /Posts/Edit/id
        /// </remarks>
        /// <param name="id">Post id (int)</param>      
        /// <param name="request">EditPostRequest object</param>
        /// <returns>Возвращает отредактированную статью</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpPatch]
        [Route("[action]/{id}")]
        [Authorize(Roles = "Модератор")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] EditPostRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return StatusCode(400, $"Ошибка: Пользователь \"{request.UserId}\" не зарегестрирован. Сначала пройдите регистрацию!");
            }

            var post = await _postsRepository.GetAsync(id);
            if (post == null)
            {
                return StatusCode(400, $"Ошибка: Статья с идентификатором \"{id}\" не существует!");
            }
                
            post = _mapper.Map<EditPostRequest, Post>(request, post);
            var allTags = await _tagsRepository.GetAllAsync();
            var tagsId = post.Tags.Select(t => t.Id);
            post.Tags = allTags.Where(t => tagsId.Contains(t.Id)).ToList();
            post.User = user;
            await _postsRepository.UpdateAsync(post);

            return StatusCode(200, $"Статья: \"{post.Id}\" обновлена!");
        }

        /// <summary>
        /// Удаление статьи по id
        /// </summary>
        /// <remarks>
        /// DELETE /Posts/Delete/id
        /// </remarks>
        /// <param name="id">Post id (int)</param>      
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
            var post = await _postsRepository.GetAsync(id);
            if (post == null)
            {
                return StatusCode(400, $"Ошибка: Статья с идентификатором \"{id}\" не существует!");
            }

            await _postsRepository.DeleteAsync(post);

            return StatusCode(200, $"Статья: с идентификатором: \"{post.Id}\" удалена!");
        }

        private async Task<List<Post>> GetAllPostsAsync()
        {
            var posts = await _postsRepository.GetAllAsync();

            return posts.ToList();
        }

        //private async Task<List<Post>> GetAllPosts(User user)
        //{
        //    var posts = _postsRepository.GetAllUserPostsAsync(user.Id);

        //    return await posts;
        //}
    }
}
