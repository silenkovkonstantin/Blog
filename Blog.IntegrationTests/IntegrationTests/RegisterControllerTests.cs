using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.IntegrationTests.IntegrationTests
{
    public class RegisterControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public RegisterControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Register_WhenCalled_ReturnsRegisterForm()
        {
            var response = await _client.GetAsync("/Register/Register");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Добавьте нового пользователя", responseString);
        }

        [Fact]
        public async Task Register_SentWrongModel_ReturnsViewWithErrorMessages()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "Register/Register");

            var formModel = new Dictionary<string, string>
            {
                { "Login", "new_user"},
                { "EmailReg", "new_user@example.com" },
                { "PasswordReg", "123" },
                { "PasswordConfirm", "456" },
                { "ImageUrl", "https://i.pinimg.com/originals/5c/9c/36/5c9c363f068fd9808161e711257b0946.jpg" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Пароли не совпадают", responseString);
        }

        [Fact]
        public async Task Register_WhenPOSTExecuted_ReturnsToIndexViewWithRegisteredUser()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "Register/Register");

            var formModel = new Dictionary<string, string>
            {
                { "Login", "new_user"},
                { "EmailReg", "new_user@example.com" },
                { "PasswordReg", "123" },
                { "PasswordConfirm", "123" },
                { "ImageUrl", "https://i.pinimg.com/originals/5c/9c/36/5c9c363f068fd9808161e711257b0946.jpg" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await _client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Welcome", responseString);
        }
    }
}
