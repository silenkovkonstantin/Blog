using Blog.ViewModels;
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
    public class AccountManagerControllerTests :
        IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public AccountManagerControllerTests(CustomWebApplicationFactory<Startup> factory)
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
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        private async Task PerformLogin(string username, string password)
        {
            var user = new LoginViewModel
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
            List<string> expectedResponse = new List<string> { "patrickbateman", "ryangosling", "jasonstatham" };
            //Act
            await PerformLogin("patrickbateman", "123456");
            var responseJson = await _client.GetStringAsync("/Users");
            List<string> actualResponse = JsonConvert.DeserializeObject<List<string>>(responseJson);
            // Assert
            Assert.Equal(expectedResponse, actualResponse);
        }
    }
}
