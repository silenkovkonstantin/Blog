using AutoMapper;
using Blog.Controllers;
using Blog.Data.Models.Db;
using Blog.Data.Repository;
using Blog.Data.UoW;
using Microsoft.AspNetCore.Mvc;
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
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private readonly PostsController _controller;

        public PostsControllerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _controller = new PostsController(_mockMapper.Object, _mockUnitOfWork.Object);
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
