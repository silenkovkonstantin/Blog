using BlogAPI;
using BlogAPI.Contracts.Models.Users;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Blog.IntegrationTests.IntegrationTests
{
    public class UsersControllerTests :
        IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public UsersControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetUsersUnauthrizedShouldReturn404()
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

            var res = await _client.PostAsJsonAsync("/Users/Login", user);
        }

        [Fact]
        public async Task CanGetUsers()
        {
            // Arrange
            JsonArray expectedResponse = new JsonArray("{\"userAmount\":3,\"users\":[{\"id\":\"f4877935-2247-4f02-a094-1e6746a64535\",\"firstName\":\"Админ\",\"lastName\":\"Админ\",\"userName\":\"admin111\",\"email\":\"admin111@example.com\",\"roles\":[\"Администратор\",\"Пользователь\"]},{\"id\":\"602c09bb-a893-42c3-ae90-c1767b37ef2b\",\"firstName\":\"Модер\",\"lastName\":\"Модер\",\"userName\":\"moder2\",\"email\":\"moder2@example.com\",\"roles\":[\"Пользователь\",\"Модератор\"]},{\"id\":\"08f6a303-88bf-474b-a688-fea52cf93810\",\"firstName\":\"Юзер\",\"lastName\":\"Юзер\",\"userName\":\"user3\",\"email\":\"user3@example.com\",\"roles\":[\"Пользователь\"]}]}");
            
            //Act
            await PerformLogin("admin111@example.com", "12345");
            var responseJson = await _client.GetStringAsync("/Users");
            JsonArray actualResponse = JsonConvert.DeserializeObject<JsonArray>(responseJson);
            // Assert
            Assert.Equal(expectedResponse, actualResponse);
        }
    }
}
