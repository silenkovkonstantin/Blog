using BlogAPI;
using BlogAPI.Contracts.Models.Users;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace Blog.IntegrationTests.IntegrationTests
{
    public class BasicTests
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public BasicTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Theory]
        [InlineData("/Posts")]
        [InlineData("/Tags")]
        public async Task Get_EndPointsReturnSuccessAndCorrectType(string url)
        {
            // Arrange
            await PerformLogin("moder2@example.com", "22222");

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
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
    }
}