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
        private IUnitOfWork _unitOfWork;

        public PostsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "Администратор")]
        [Route("Posts")]
        [HttpPost]
        public async Task<IActionResult> Posts()
        {
            var repository = _unitOfWork.GetRepository<Post>() as PostsRepository;
            var posts = await GetAllPostsAsync();
            var model = _mapper.Map<IEnumerable<Post>, PostsViewModel>(posts);
            
            return View("Posts", model);
        }

        [Authorize(Roles = "Администратор")]
        [Route("NewPost")]
        [HttpPost]
        public async Task<IActionResult> NewPost(PostsViewModel postsvm)
        {
            var repository = _unitOfWork.GetRepository<Post>() as PostsRepository;
            var post = _mapper.Map<PostsViewModel, Post>(postsvm);
            await repository.CreateAsync(post);
            var posts = await GetAllPostsAsync();
            var model = _mapper.Map<IEnumerable<Post>, PostsViewModel>(posts);

            return View("Posts", model);
        }

        [Authorize(Roles = "Администратор")]
        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(PostsViewModel postsvm)
        {
            var repository = _unitOfWork.GetRepository<Post>() as PostsRepository;
            var post = _mapper.Map<PostsViewModel, Post>(postsvm);
            await repository.UpdateAsync(post);
            var posts = await GetAllPostsAsync();
            var model = _mapper.Map<IEnumerable<Post>, PostsViewModel>(posts);

            return View("Posts", model);
        }

        [Authorize(Roles = "Администратор")]
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
