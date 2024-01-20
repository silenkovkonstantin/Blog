using AutoMapper;
using Blog.Controllers;
using Blog.Data.Models.Db;
using Blog.Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.UnitTests.Controllers
{
    public class CommentsControllerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IRepository<Post>> _mockPostsRepository;
        private readonly Mock<ILogger<CommentsController>> _mockLogger;
        private readonly Mock<IRepository<Comment>> _mockCommentsRepository;
        private Mock<IMapper> _mockMapper;
        private readonly CommentsController _controller;

        public CommentsControllerTests()
        {
            _mockUserManager = new Mock<UserManager<User>>();
            _mockPostsRepository = new Mock<IRepository<Post>>();
            _mockLogger = new Mock<ILogger<CommentsController>>();
            _mockCommentsRepository = new Mock<IRepository<Comment>>();
            _mockMapper = new Mock<IMapper>();
            _controller = new CommentsController(_mockMapper.Object, _mockUserManager.Object, _mockCommentsRepository.Object,
                _mockPostsRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public void Comments_ActionExecutes_ReturnsViewForComments()
        {
            // Act
            var result = _controller.Comments();
            // Assert
            Assert.IsType<Task<IActionResult>>(result);
        }
    }
}
