using AutoMapper;
using Blog.Data.Repository;
using Blog.Data.UoW;
using Blog.Data.Models.Db;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using NuGet.Protocol.Core.Types;

namespace Blog.Controllers
{
    public class CommentsController : Controller
    {
        private IMapper _mapper;
        private IUnitOfWork _unitOfWork;

        public CommentsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "Администратор")]
        [Route("Comments")]
        [HttpPost]
        public async Task<IActionResult> Comments()
        {
            var repository = _unitOfWork.GetRepository<Comment>() as CommentsRepository;
            var comments = await GetAllCommentsAsync();
            var model = _mapper.Map<IEnumerable<Comment>, CommentsViewModel>(comments);

            return View("Comments", model);
        }

        [Authorize(Roles = "Администратор")]
        [Route("NewComment")]
        [HttpPost]
        public async Task<IActionResult> NewComment(CommentsViewModel commentsvm)
        {
            var repository = _unitOfWork.GetRepository<Comment>() as CommentsRepository;
            var comment = _mapper.Map<CommentsViewModel, Comment>(commentsvm);
            await repository.CreateAsync(comment);
            var comments = await repository.GetAllPostCommentsAsync(comment.PostId);
            var model = _mapper.Map<IEnumerable<Comment>, CommentsViewModel>(comments);

            return View("Comments", model);
        }

        [Authorize(Roles = "Администратор")]
        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(CommentsViewModel commentsvm)
        {
            var repository = _unitOfWork.GetRepository<Comment>() as CommentsRepository;
            var comment = _mapper.Map<CommentsViewModel, Comment>(commentsvm);
            await repository.UpdateAsync(comment);
            var comments = await repository.GetAllPostCommentsAsync(comment.PostId);
            var model = _mapper.Map<IEnumerable<Comment>, CommentsViewModel>(comments);

            return View("Comments", model);
        }

        [Authorize(Roles = "Администратор")]
        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id, CommentsViewModel commentsvm)
        {
            var repository = _unitOfWork.GetRepository<Comment>() as CommentsRepository;
            var comment = await GetCommentAsync(id);
            await repository.DeleteAsync(comment);
            var comments = await repository.GetAllPostCommentsAsync(comment.PostId);
            var model = _mapper.Map<IEnumerable<Comment>, CommentsViewModel>(comments);

            return View("Comments", model);
        }

        private async Task<List<Comment>> GetAllCommentsAsync()
        {
            var repository = _unitOfWork.GetRepository<Comment>() as CommentsRepository;
            var comments = await repository.GetAllAsync();

            return comments.ToList();
        }

        private async Task<Comment> GetCommentAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Comment>() as CommentsRepository;
            var comment = await repository.GetAsync(id);

            return comment;
        }
    }
}
