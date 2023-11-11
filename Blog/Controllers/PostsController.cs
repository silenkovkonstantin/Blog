using AutoMapper;
using Blog.Data.Repository;
using Blog.Data.UoW;
using Blog.Extensions;
using Blog.Models.Db;
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
        private readonly SignInManager<User> _signInManager;
        private IUnitOfWork _unitOfWork;

        public PostsController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "Администратор")]
        [Route("Posts")]
        [HttpPost]
        public async Task<IActionResult> Posts()
        {
            var repository = _unitOfWork.GetRepository<Post>() as PostsRepository;
            var posts = await repository.GetAllAsync();
            var model = new PostsViewModel()
            {
                Posts = posts.OrderBy(x => x.CreatedDate).ToList(),
            };
            
            return View("Posts", model);
        }

        [Authorize(Roles = "Администратор")]
        [Route("NewPost")]
        [HttpPost]
        public async Task<IActionResult> NewPost(PostsViewModel postsvm)
        {
            var currentuser = User;
            var result = await _userManager.GetUserAsync(currentuser);
            var repository = _unitOfWork.GetRepository<Post>() as PostsRepository;
            
            var item = new Post()
            {
                Author = result,
                Title = postsvm.NewPost.Title,
                Description = postsvm.NewPost.Description,
                Text = postsvm.NewPost.Text,
            };
            
            await repository.CreateAsync(item);
            var posts = await repository.GetAllAsync();
            
            var model = new PostsViewModel()
            {
                Posts = posts.OrderBy(x => x.CreatedDate).ToList(),
            };

            return View("Posts", model);
        }

        [Authorize(Roles = "Администратор")]
        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(int id, PostsViewModel postsvm)
        {
            var currentuser = User;
            var result = await _userManager.GetUserAsync(currentuser);
            var repository = _unitOfWork.GetRepository<Post>() as PostsRepository;
            
            var item = await repository.GetAsync(id);
            item.Title = postsvm.NewPost.Title;
            item.Description = postsvm.NewPost.Description;
            item.Text = postsvm.NewPost.Text;
            
            await repository.UpdateAsync(item);
            var posts = await repository.GetAllAsync();

            var model = new PostsViewModel()
            {
                Posts = posts.OrderBy(x => x.CreatedDate).ToList(),
            };

            return View("Posts", model);
        }

        [Authorize(Roles = "Администратор")]
        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id, PostsViewModel postsvm)
        {
            var currentuser = User;
            var result = await _userManager.GetUserAsync(currentuser);
            var postsRepository = _unitOfWork.GetRepository<Post>() as PostsRepository;
            var commentsRepository = _unitOfWork.GetRepository<Comment>() as CommentsRepository;

            var post = await postsRepository.GetAsync(id);

            foreach (var comment in post.Comments)
                await commentsRepository.DeleteAsync(comment);

            await postsRepository.DeleteAsync(post);
            var posts = await postsRepository.GetAllAsync();

            var model = new PostsViewModel()
            {
                Posts = posts.OrderBy(x => x.CreatedDate).ToList(),
            };

            return View("Posts", model);
        }

        private async Task<List<Post>> GetAllPosts()
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
