using BlogAPI;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Blog.IntegrationTests.IntegrationTests
{
    public class BasicTests
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public BasicTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Home/Index")]
        [InlineData("/Posts")]
        [InlineData("/Comments")]
        [InlineData("/Tags")]
        public async Task Get_EndPointsReturnSuccessAndCorrectType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}