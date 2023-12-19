using AutoMapper;
using Blog.Controllers;
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
    public class TagsControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private readonly TagsController _controller;

        public TagsControllerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _controller = new TagsController(_mockMapper.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public void Tags_ActionExecutes_ReturnsViewForTags()
        {
            // Act
            var result = _controller.Tags();
            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}
