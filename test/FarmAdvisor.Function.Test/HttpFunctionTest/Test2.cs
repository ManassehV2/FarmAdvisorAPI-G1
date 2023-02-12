using FarmAdvisor.Models.Models;
using FarmAdvisor_HttpFunctions.Functions;
using FarmAdvisor.DataAccess.MSSQL.Functions.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using FarmAdvisor_HttpFunctions.Functionsw;

namespace FarmAdvisor_HttpFunctions.Tests
{
    public class FarmEndpointTests
    {

        [Fact]
        public async Task GetFarmModel_ReturnsOkResult_WhenDataIsAvailable()
        {
            // Arrange
            var farmId = Guid.NewGuid();
            var farm = new FarmModel { FarmId = farmId, Name = "Test Farm", Postcode = "1234", City = "Test City", Country = "Test Country", UserId = Guid.NewGuid() };

            var mockCrud = new Mock<ICrud>();
            mockCrud.Setup(x => x.Find<FarmModel>(farmId)).ReturnsAsync(farm);

            var farmEndpoint = new FarmEndpoint(mockCrud.Object);
            var mockLogger = new Mock<ILogger>();

            // Act
            var result = await farmEndpoint.GetFarmModel(null, farmId, mockLogger.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<FarmModel>(okResult.Value);
            Assert.Equal(farm, returnValue);
        }
        

    }
}
