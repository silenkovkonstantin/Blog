using AutoMapper;
using Blog.Controllers;
using Blog.Data.Models.Db;
using Blog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.UnitTests.Controllers
{
    public class RegisterControllerTests
    {
        private Mock<IMapper> _mockMapperr;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<SignInManager<User>> _mockSignInManager;
        private readonly RegisterController _controller;

        public RegisterControllerTests()
        {
            _mockMapperr = new Mock<IMapper>();
            _mockUserManager = new Mock<UserManager<User>>();
            _mockSignInManager = new Mock<SignInManager<User>>();
            _controller = new RegisterController(_mockMapperr.Object, _mockUserManager.Object, _mockSignInManager.Object);
        }

        [Fact]
        public void Register_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Login", "Поле Имя обязательно для заполнения");
            var formModel = new RegisterViewModel
            {
                Login = "new_user",
                EmailReg = "new_user@example.com",
                PasswordReg = "123",
                PasswordConfirm = "123",
                ImageUrl = "https://i.pinimg.com/originals/5c/9c/36/5c9c363f068fd9808161e711257b0946.jpg"
            };
            // Act
            var result = _controller.Register(formModel);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void Register_RegisterUserAndReturnsARedirect_WhenModelStateIsValid()
        {
            // Arrange
            var formModel = new RegisterViewModel
            {
                Login = "new_user",
                EmailReg = "new_user@example.com",
                PasswordReg = "123",
                PasswordConfirm = "123",
                ImageUrl = "https://i.pinimg.com/originals/5c/9c/36/5c9c363f068fd9808161e711257b0946.jpg"
            };
            // Act
            var result = _controller.Register(formModel);
            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _mockUserManager.Verify();
        }
    }
}
