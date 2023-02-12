using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FarmAdvisor.HttpFunctions.Functions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FarmAdvisor.HttpFunctions.Tests
{
    public class FarmFieldFunctionsTests
    {
        private readonly Mock<ILogger<FarmFieldFunctions>> _loggerMock;
        private readonly FarmFieldFunctions _farmFieldFunctions;
        private readonly Mock<HttpRequest> _requestMock;

        public FarmFieldFunctionsTests()
        {
            _loggerMock = new Mock<ILogger<FarmFieldFunctions>>();
            _farmFieldFunctions = new FarmFieldFunctions(_loggerMock.Object);
            _requestMock = new Mock<HttpRequest>();
        }

        [Fact]
        public async Task GetAllFarmFields_ValidId_ReturnsOkResult()
        {
            // Arrange
            var farmId = Guid.NewGuid().ToString();
            _requestMock.Setup(x => x.Query["farmId"]).Returns(farmId);

            // Act
            var result = await _farmFieldFunctions.GetAllFarmFields(_requestMock.Object);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllFarmFields_InvalidId_ReturnsBadRequestResult()
        {
            // Arrange
            var invalidId = "invalidId";
            _requestMock.Setup(x => x.Query["farmId"]).Returns(invalidId);

            // Act
            var result = await _farmFieldFunctions.GetAllFarmFields(_requestMock.Object);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
