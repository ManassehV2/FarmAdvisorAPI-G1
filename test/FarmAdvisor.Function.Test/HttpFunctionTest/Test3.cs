using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FarmAdvisor.Models.Models;
using FarmAdvisor.DataAccess.MSSQL.Functions.Interfaces;
using Newtonsoft.Json;

namespace FarmAdvisor_HttpFunctions.Functions.Tests
{
    public class UserApiTests
    {
        [Fact]
        public async Task AddUser_ReturnsOkResult_WhenGivenValidInput()
        {
            // Arrange
            var user = new UserModel { Name = "John Doe", Phone = "123456", Email = "johndoe@email.com", AuthId = "123456789" };
            var request = new Mock<HttpRequest>();
            request.Setup(r => r.Body).Returns(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(user))));

            var logger = new Mock<ILogger>();
            var crud = new Mock<ICrud>();
            crud.Setup(c => c.Create(It.IsAny<UserModel>())).ReturnsAsync(user);

            var userApi = new UserApi(crud.Object);

            // Act
            var result = await userApi.AddUser(request.Object, logger.Object);

            // Assert
            Assert.IsType<OkObjectResult>(result);
           // var okResult = result as OkObjectResult;
            //Assert.Equal(200, okResult.StatusCode);
           // Assert.Equal(user, okResult.Value);
        }

        [Fact]
        public async Task AddUser_ReturnsConflictResult_WhenPhoneExists()
        {
            // Arrange
            var user = new UserModel { Name = "John Doe", Phone = "123456", Email = "johndoe@email.com", AuthId = "123456789" };
            var request = new Mock<HttpRequest>();
            request.Setup(r => r.Body).Returns(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(user))));

            var logger = new Mock<ILogger>();
            var crud = new Mock<ICrud>();
            crud.Setup(c => c.Create(It.IsAny<UserModel>())).ThrowsAsync(new Exception("Phone exists"));

            var userApi = new UserApi(crud.Object);

            // Act
            var result = await userApi.AddUser(request.Object, logger.Object);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
           // var conflictResult = result as ConflictObjectResult;
           // Assert.Equal(409, conflictResult.StatusCode);
         //   Assert.Equal("Phone exists", conflictResult.Value);
        }

    }
}
