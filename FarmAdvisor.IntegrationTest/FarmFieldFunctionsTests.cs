using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FarmAdvisor.HttpFunctions.Tests.Functions
{
    public class FarmFieldFunctionsTests
    {
        private readonly HttpClient _client;

        public FarmFieldFunctionsTests()
        {
            _client = new HttpClient();
        }

        [Fact]
        public async Task GetAllFarmFields_Should_Return_OK_With_Valid_FarmId()
        {
            // Arrange
            string farmId = Guid.NewGuid().ToString();
            string requestUrl = $"http://localhost:7071/api/users/farms/fields?farmId={farmId}";

            // Act
            var response = await _client.GetAsync(requestUrl);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllFarmFields_Should_Return_BadRequest_With_Invalid_FarmId()
        {
            // Arrange
            string farmId = "invalid";
            string requestUrl = $"http://localhost:7071/api/users/farms/fields?farmId={farmId}";

            // Act
            var response = await _client.GetAsync(requestUrl);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
