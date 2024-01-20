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
    public class PostsControllerTests
    {
        private Mock<IMapper> _mockMapper;
        private Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IRepository<Post>> _mockPostsRepository;
        private readonly Mock<IRepository<Tag>> _mockTagsRepository;
        private readonly Mock<ILogger<PostsController>> _mockLogger;
        private readonly PostsController _controller;

        public PostsControllerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockUserManager = new Mock<UserManager<User>>();
            _mockPostsRepository = new Mock<IRepository<Post>>();
            _mockTagsRepository = new Mock<IRepository<Tag>>();
            _mockLogger = new Mock<ILogger<PostsController>>();
            _controller = new PostsController(_mockMapper.Object, _mockUserManager.Object, _mockPostsRepository.Object,
                _mockTagsRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public void Posts_ActionExecutes_ReturnsViewForPosts()
        {
            // Act
            var result = _controller.Posts();
            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}
