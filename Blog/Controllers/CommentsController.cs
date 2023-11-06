using AutoMapper;
using Blog.Data.Repository;
using Blog.Data.UoW;
using Blog.Models.Db;
using Blog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace Blog.Controllers
{
    public class CommentsController : Controller
    {
        private IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private IUnitOfWork _unitOfWork;

        public CommentsController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [Route("Comments")]
        [HttpPost]
        public async Task<IActionResult> Comments()
        {
            var repository = _unitOfWork.GetRepository<Comment>() as CommentsRepository;
            var comments = await repository.GetAllAsync();
            var model = new CommentsViewModel()
            {
                Comments = comments.OrderBy(x => x.CreatedDate).ToList(),
            };

            return View("Comments", model);
        }

        [Route("NewComment")]
        [HttpPost]
        public async Task<IActionResult> NewComment(int postid, CommentsViewModel commentsvm)
        {
            var currentuser = User;
            var user = await _userManager.GetUserAsync(currentuser);
            var postsRepository = _unitOfWork.GetRepository<Post>() as PostsRepository;
            var commentsRepository = _unitOfWork.GetRepository<Comment>() as CommentsRepository;

            var comment = new Comment()
            {
                Author = user,
                Text = commentsvm.NewComment.Text,
            };

            await commentsRepository.CreateAsync(comment);
            var comments = await commentsRepository.GetAllPostCommentsAsync(postid);

            var model = new CommentsViewModel()
            {
                Comments = comments.OrderBy(x => x.CreatedDate).ToList(),
            };

            return View("Comments", model);
        }

        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(int id, CommentsViewModel commentsvm)
        {
            var currentuser = User;
            var user = await _userManager.GetUserAsync(currentuser);
            var repository = _unitOfWork.GetRepository<Comment>() as CommentsRepository;

            var comment = await repository.GetAsync(id);
            comment.Text = commentsvm.NewComment.Text;

            await repository.UpdateAsync(comment);
            var comments = await repository.GetAllPostCommentsAsync(comment.PostId);

            var model = new CommentsViewModel()
            {
                Comments = comments.OrderBy(x => x.CreatedDate).ToList(),
            };

            return View("Comments", model);
        }

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
