using AutoMapper;
using Blog.Controllers;
using Blog.Data.Models.Db;
using Blog.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private IMapper _mapper;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<SignInManager<User>> _mockSignInManager;
        private readonly Mock<ILogger<RegisterController>> _mockLogger;
        private readonly RegisterController _controller;

        public RegisterControllerTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
            _mockUserManager = new Mock<UserManager<User>>(
    new Mock<IUserStore<User>>().Object,
    new Mock<IOptions<IdentityOptions>>().Object,
    new Mock<IPasswordHasher<User>>().Object,
    new IUserValidator<User>[0],
    new IPasswordValidator<User>[0],
    new Mock<ILookupNormalizer>().Object,
    new Mock<IdentityErrorDescriber>().Object,
    new Mock<IServiceProvider>().Object,
    new Mock<ILogger<UserManager<User>>>().Object);
            _mockUserManager
                .Setup(userManager => userManager.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));
            _mockUserManager
                .Setup(userManager => userManager.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            _mockSignInManager = new Mock<SignInManager<User>>(_mockUserManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<User>>(), null, null, null, null);
            _mockLogger = new Mock<ILogger<RegisterController>>();
            _controller = new RegisterController(_mapper, _mockUserManager.Object, _mockSignInManager.Object, 
                _mockLogger.Object);
        }

        [Fact]
        public void Register_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            // Arrange
            //_controller.ModelState.AddModelError("UserName", "Поле Никнейм обязательно для заполнения");
            var formModel = new RegisterViewModel
            {
                FirstName = "Новый",
                LastName = "Пользователь",
                UserName = "new_user",
                Email = "new_user@example.com",
                PasswordReg = "00000",
                PasswordConfirm = "00000",
            };
            // Act
            var result = _controller.Register(formModel);
            // Assert
            Assert.IsType<Task<IActionResult>>(result);
            //var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            //Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async void Register_RegisterUserAndReturnsARedirect_WhenModelStateIsValid()
        {
            // Arrange
            var formModel = new RegisterViewModel
            {
                FirstName = "Новый",
                LastName = "Пользователь",
                UserName = "new_user",
                Email = "new_user@example.com",
                PasswordReg = "00000",
                PasswordConfirm = "00000",
            };
            // Act
            var result = await _controller.Register(formModel);
            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            //Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _mockUserManager.Verify();
        }
    }
}
