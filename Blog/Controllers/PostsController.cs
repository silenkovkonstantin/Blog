using AutoMapper;
using Blog.Data.Repository;
using Blog.Data.Models.Db;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class PostsController : Controller
    {
        private IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private IRepository<Post> _postsRepository;
        private IRepository<Tag> _tagsRepository;
        private readonly ILogger<PostsController> _logger;

        public PostsController(IMapper mapper, UserManager<User> userManager, IRepository<Post> postsRepository,
            IRepository<Tag> tagsRepository, ILogger<PostsController> logger)
        {
            _mapper = mapper;
            _userManager = userManager;
            _postsRepository = postsRepository;
            _tagsRepository = tagsRepository;
            _logger = logger;
        }

        [Route("Posts")]
        [HttpGet]
        public async Task<IActionResult> Posts()
        {
            var posts = await GetAllPostsAsync();

            return View("Posts", posts);
        }

        [HttpGet]
        public async Task<IActionResult> Post(int id)
        {
            var post = await _postsRepository.GetAsync(id);
            var postvm = _mapper.Map<Post, PostViewModel>(post);
            //var author = await _userManager.FindByIdAsync(post.UserId);
            //postvm.User = _mapper.Map<User, UserViewModel>(author);
            return View("Post", postvm);
        }

        [Authorize(Roles = "Пользователь")]
        [Route("NewPost")]
        [HttpGet]
        public async Task<IActionResult> NewPost()
        {
            var postvm = new PostViewModel();
            var user = await _userManager.GetUserAsync(User);
            postvm.UserId = user.Id;
            var tags = await _tagsRepository.GetAllAsync();
            postvm.Tags = tags.Select(t => new TagViewModel { Name = t.Name , Id = t.Id }).ToList();

            return View("NewPost", postvm);
        }

        [Authorize(Roles = "Пользователь")]
        [Route("NewPost")]
        [HttpPost]
        public async Task<IActionResult> NewPost(PostViewModel postvm)
        { 
            var post = _mapper.Map<PostViewModel, Post>(postvm);
            var allTags = await _tagsRepository.GetAllAsync();
            var tagsId = post.Tags.Select(t => t.Id);
            post.Tags = allTags.Where(t => tagsId.Contains(t.Id)).ToList();
            await _postsRepository.CreateAsync(post);
            _logger.LogInformation($"Пользователь {post.UserId} добавил новую статью {post.Id}");
            var posts = await GetAllPostsAsync();

            return View("Posts", posts);
        }

        [Authorize(Roles = "Модератор")]
        [Route("PostEdit")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _postsRepository.GetAsync(id);
            var postvm = _mapper.Map<Post, PostViewModel>(post);
            var tags = await _tagsRepository.GetAllAsync();
            postvm.Tags = tags.Select(t => new TagViewModel { Name = t.Name, Id = t.Id }).ToList();

            return View("PostEdit", postvm);
        }

        [Authorize(Roles = "Модератор")]
        [Route("PostEdit")]
        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel postvm)
        {
            var post = await _postsRepository.GetAsync(postvm.Id);
            post = _mapper.Map<PostViewModel, Post>(postvm, post);
            var allTags = await _tagsRepository.GetAllAsync();
            var tagsId = post.Tags.Select(t => t.Id);
            post.Tags = allTags.Where(t => tagsId.Contains(t.Id)).ToList();
            var user = await _userManager.GetUserAsync(User);
            post.User = user;
            await _postsRepository.UpdateAsync(post);
            _logger.LogInformation($"Отредактирована статья {post.Id}");
            var posts = await GetAllPostsAsync();

            return View("Posts", posts);
        }

        [Authorize(Roles = "Модератор")]
        [Route("PostDelete")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postsRepository.GetAsync(id);
            await _postsRepository.DeleteAsync(post);
            var posts = await GetAllPostsAsync();
            _logger.LogInformation($"Удалена статья {post.Id}");

            return View("Posts", posts);
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
