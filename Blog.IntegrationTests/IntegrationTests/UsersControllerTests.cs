using BlogAPI;
using BlogAPI.Contracts.Models.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Blog.IntegrationTests.IntegrationTests
{
    public class UsersControllerTests :
        IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public UsersControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetUsersUnauthrizedShouldReturn401()
        {
            // Act
            var response = await _client.GetAsync("/Users");
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        private async Task PerformLogin(string username, string password)
        {
            var user = new LoginRequest
            {
                Email = username,
                Password = password
            };

            var res = await _client.PostAsJsonAsync("/Login", user);
        }

        [Fact]
        public async Task CanGetUsers()
        {
            // Arrange
            List<string> expectedResponse = new List<string> { "admin111", "moder2", "user3" };
            //Act
            await PerformLogin("admin111@example.com", "12345");
            var responseJson = await _client.GetStringAsync("/Users");
            List<string> actualResponse = JsonConvert.DeserializeObject<List<string>>(responseJson);
            // Assert
            Assert.Equal(expectedResponse, actualResponse);
        }
    }
}
