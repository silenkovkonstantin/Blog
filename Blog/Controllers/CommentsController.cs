using AutoMapper;
using Blog.Data.Repository;
using Blog.Data.Models.Db;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class CommentsController : Controller
    {
        private IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private IRepository<Comment> _commentsRepository;
        private IRepository<Post> _postsRepository;
        private readonly ILogger<CommentsController> _logger;

        public CommentsController(IMapper mapper, UserManager<User> userManager, IRepository<Comment> commentsRepository, 
            IRepository<Post> postsRepository, ILogger<CommentsController> logger)
        {
            _mapper = mapper;
            _userManager = userManager;
            _commentsRepository = commentsRepository;
            _postsRepository = postsRepository;
            _logger = logger;
        }

        [Authorize(Roles = "Модератор")]
        [Route("Comments")]
        [HttpGet]
        public async Task<IActionResult> Comments()
        {
            var comments = await GetAllCommentsAsync();

            return View("Comments", comments);
        }

        [Authorize(Roles = "Администратор, Модератор, Пользователь")]
        [Route("NewComment")]
        [HttpPost]
        public async Task<IActionResult> NewComment(CommentViewModel commentvm)
        {
            var user = await _userManager.GetUserAsync(User);
            var comment = _mapper.Map<CommentViewModel, Comment>(commentvm);
            comment.User = user;
            comment.UserId = user.Id;
            await _commentsRepository.CreateAsync(comment);
            _logger.LogInformation($"Пользователь {comment.UserId} добавил новый комментарий {comment.Id}");

            return RedirectToAction("Post", "Posts", new { id = comment.PostId });
        }

        [Authorize(Roles = "Модератор")]
        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(CommentViewModel commentvm)
        {
            var comment = _mapper.Map<CommentViewModel, Comment>(commentvm);
            await _commentsRepository.UpdateAsync(comment);
            _logger.LogInformation($"Изменен комментарий {comment.Id}");

            return RedirectToAction("Post", "Posts", new { id = comment.PostId });
        }

        [Authorize(Roles = "Модератор")]
        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await GetCommentAsync(id);
            await _commentsRepository.DeleteAsync(comment);
            _logger.LogInformation($"Удален комментарий {comment.Id}");

            return RedirectToAction("Post", "Posts", new { id = comment.PostId });
        }

        private async Task<List<Comment>> GetAllCommentsAsync()
        {
            var comments = await _commentsRepository.GetAllAsync();
            return comments.ToList();
        }

        private async Task<Comment> GetCommentAsync(int id)
        {
            var comment = await _commentsRepository.GetAsync(id);
            return comment;
        }
    }
}
