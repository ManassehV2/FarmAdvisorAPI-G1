using System.Text;
using FarmAdvisor.Models.Models;
using Newtonsoft.Json;

namespace FarmAdvisor_HttpFunctions.Tests
{
    public class FieldEndpointIntegrationTests
    {
        private readonly HttpClient _client;

        public FieldEndpointIntegrationTests()
        {
            _client = new HttpClient();
        }

        [Fact]
        public async Task AddFieldModel_ShouldReturnOkResult()
        {
            // Arrange
            var url = "https://localhost:7071/api/AddFieldEndpoint";
            var farmId = Guid.NewGuid();
            var field = new FieldModel
            {
                FarmId = farmId,
                Name = "Field 1",
                Alt = 1000,
                Polygon = "POLYGON((0 0, 0 1, 1 1, 1 0, 0 0))"
            };

            // Act
            var response = await _client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(field), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<FieldModel>(content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(field.Name, result.Name);
            Assert.Equal(field.Alt, result.Alt);
            Assert.Equal(field.Polygon, result.Polygon);
        }

        [Fact]
        public async Task GetFieldModel_ShouldReturnOkResult()
        {
            // Arrange
            var url = "https://localhost:7071/api/FieldApi/{id}";
            var id = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync(string.Format(url, id));
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<FieldModel>(content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(id, result.FieldId);
        }
    }
}
