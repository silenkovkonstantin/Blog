using AutoMapper;
using Blog.Controllers;
using Blog.Data.Models.Db;
using Blog.Data.Repository;
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
    public class TagsControllerTests
    {
        private Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<Tag>> _mockTagsRepository;
        private readonly Mock<ILogger<TagsController>> _mockLogger;
        private readonly TagsController _controller;

        public TagsControllerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockTagsRepository = new Mock<IRepository<Tag>>();
            _mockLogger = new Mock<ILogger<TagsController>>();
            _controller = new TagsController(_mockMapper.Object, _mockTagsRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public void Tags_ActionExecutes_ReturnsViewForTags()
        {
            // Act
            var result = _controller.Tags();
            // Assert
            Assert.IsType<Task<IActionResult>>(result);
        }
    }
}
