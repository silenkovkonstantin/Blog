﻿using AutoMapper;
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
    public class CommentsControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private readonly CommentsController _controller;

        public CommentsControllerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _controller = new CommentsController(_mockMapper.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public void Comments_ActionExecutes_ReturnsViewForComments()
        {
            // Act
            var result = _controller.Comments();
            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}
