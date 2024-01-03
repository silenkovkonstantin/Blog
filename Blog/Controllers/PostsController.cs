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
        private IUnitOfWork _unitOfWork;

        public PostsController(IMapper mapper, UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
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
            var repository = _unitOfWork.GetRepository<Post>() as PostsRepository;
            var post = await repository.GetAsync(id);
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
            postvm.User = _mapper.Map<User, UserViewModel>(user);
            postvm.UserId = user.Id;
            var tagsRepository = _unitOfWork.GetRepository<Tag>() as TagsRepository;
            var tags = await tagsRepository.GetAllAsync();
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
            var repository = _unitOfWork.GetRepository<Post>() as PostsRepository;
            await repository.CreateAsync(post);
            var posts = await GetAllPostsAsync();
            
            //var model = _mapper.Map<IEnumerable<Post>, PostsViewModel>(posts);

            return View("Posts", posts);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [Route("PostEdit")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var repository = _unitOfWork.GetRepository<Post>() as PostsRepository;
            var post = await repository.GetAsync(id);
            var postvm = _mapper.Map<Post, PostViewModel>(post);
            //var user = await _userManager.GetUserAsync(User);
            //postvm.User = _mapper.Map<User, UserViewModel>(user);
            var tagsRepository = _unitOfWork.GetRepository<Tag>() as TagsRepository;
            var tags = await tagsRepository.GetAllAsync();
            postvm.Tags = tags.Select(t => new TagViewModel { Name = t.Name, IsChecked = false }).ToList();

            return View("PostEdit", postvm);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [Route("PostEdit")]
        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel postvm)
        {
            var repository = _unitOfWork.GetRepository<Post>() as PostsRepository;
            var post = _mapper.Map<PostViewModel, Post>(postvm);
            await repository.UpdateAsync(post);
            var posts = await GetAllPostsAsync();

            return View("Posts", posts);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id, PostsViewModel postsvm)
        {
            var postsRepository = _unitOfWork.GetRepository<Post>() as PostsRepository;
            var commentsRepository = _unitOfWork.GetRepository<Comment>() as CommentsRepository;
            var post = await postsRepository.GetAsync(id);

            foreach (var comment in post.Comments)
                await commentsRepository.DeleteAsync(comment);

            await postsRepository.DeleteAsync(post);
            var posts = await GetAllPostsAsync();
            var model = _mapper.Map<IEnumerable<Post>, PostsViewModel>(posts);

            return View("Posts", model);
        }

        private async Task<List<Post>> GetAllPostsAsync()
        {
            var repository = _unitOfWork.GetRepository<Post>() as PostsRepository;
            var posts = await repository.GetAllAsync();

            return posts.ToList();
        }

        private async Task<List<Post>> GetAllPosts(User user)
        {
            var repository = _unitOfWork.GetRepository<Post>() as PostsRepository;
            var posts = repository.GetAllUserPostsAsync(user.Id);

            return await posts;
        }
    }
}
