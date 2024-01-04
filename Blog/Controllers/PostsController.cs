using AutoMapper;
using Blog.Data.Repository;
using Blog.Data.UoW;
using Blog.Extensions;
using Blog.Data.Models.Db;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        public PostsController(IMapper mapper, UserManager<User> userManager, IRepository<Post> postsRepository, IRepository<Tag> tagsRepository)
        {
            _mapper = mapper;
            _userManager = userManager;
            _postsRepository = postsRepository;
            _tagsRepository = tagsRepository;
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

        [Authorize(Roles = "Администратор, Модератор")]
        [Route("NewPost")]
        [HttpGet]
        public async Task<IActionResult> NewPost()
        {
            var postvm = new PostViewModel();
            var user = await _userManager.GetUserAsync(User);
            //postvm.User = _mapper.Map<User, UserViewModel>(user);
            //postvm.User = user;
            postvm.UserId = user.Id;
            var tags = await _tagsRepository.GetAllAsync();
            postvm.Tags = tags.Select(t => new TagViewModel { Name = t.Name , Id = t.Id }).ToList();

            return View("NewPost", postvm);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [Route("NewPost")]
        [HttpPost]
        public async Task<IActionResult> NewPost(PostViewModel postvm)
        {
            //var user = await _userManager.GetUserAsync(User);
            
            var post = _mapper.Map<PostViewModel, Post>(postvm);
            //post.User = user;
            //post.UserId = user.Id;
            var allTags = await _tagsRepository.GetAllAsync();
            var tagsId = post.Tags.Select(t => t.Id);
            post.Tags = allTags.Where(t => tagsId.Contains(t.Id)).ToList();
            await _postsRepository.CreateAsync(post);
            var posts = await GetAllPostsAsync();
            
            //var model = _mapper.Map<IEnumerable<Post>, PostsViewModel>(posts);

            return View("Posts", posts);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [Route("PostEdit")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _postsRepository.GetAsync(id);
            var postvm = _mapper.Map<Post, PostViewModel>(post);
            //var user = await _userManager.GetUserAsync(User);
            //postvm.UserId = user.Id;
            var tags = await _tagsRepository.GetAllAsync();
            postvm.Tags = tags.Select(t => new TagViewModel { Name = t.Name, Id = t.Id }).ToList();

            return View("PostEdit", postvm);
        }

        [Authorize(Roles = "Администратор, Модератор")]
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
            var posts = await GetAllPostsAsync();

            return View("Posts", posts);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id, PostsViewModel postsvm)
        {
            var post = await _postsRepository.GetAsync(id);

            //foreach (var comment in post.Comments)
            //    await _commentsRepository.DeleteAsync(comment);

            await _postsRepository.DeleteAsync(post);
            var posts = await GetAllPostsAsync();
            var model = _mapper.Map<IEnumerable<Post>, PostsViewModel>(posts);

            return View("Posts", model);
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
