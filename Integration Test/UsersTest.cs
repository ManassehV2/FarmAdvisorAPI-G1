using System.Net;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Hosting;

using Microsoft.Extensions.Logging;
using FarmAdvisor.HttpFunctions;
using FarmAdvisor_HttpFunctions.Functions;
using FarmAdvisor.DataAccess.MSSQL.Functions.Interfaces;

using FarmAdvisor.Models.Models;

namespace FarmAdvisor.IntegrationTest
{
    public sealed class TestUserFunctions
    {
        readonly UserApi _userFunctions;
        public TestUserFunctions()
        {
            var startup = new HttpFunctionStartup();
            var host = new HostBuilder()
                .ConfigureWebJobs(startup.Configure)
                .Build();

            _userFunctions = new UserApi(host.Services.GetRequiredService<ICrud>());
        }

        

        [Fact]
        public async Task AddUserTest()
        {
           
            var httpContext = new DefaultHttpContext();
            
            var request = httpContext.Request;
            var jsonResult = await _userFunctions.AddUser(request);
            var result = (OkObjectResult)jsonResult.Result!;
            var final = (UserModel)result?.Value!;


            Assert.Equal("user", final?.Name);
            Assert.Equal("user@test.com", final?.Email);
            Assert.Equal("token", final?.AuthId);
            Assert.True(final?.UserID is Guid);
           // Assert.Equal(final.Name, "user");
        }
        public readonly ILogger log;
        [Fact ]  
        public async Task GetUserNewTest()
        {
            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            var jsonResult = await _userFunctions.GetUserNew(request,"token");
            var result = (HttpStatusCode)jsonResult.GetType().GetProperty("StatusCode")
                .GetValue(jsonResult, null);
            Assert.Equal(200, (int)result);
            
        }
        [Fact]
        public async Task EditUserTest()
        {
            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            var jsonResult = await _userFunctions.EditUser(request, "token");
            var result = (NotFoundObjectResult)jsonResult.Result!;
            Assert.Equal(404, (int)result.StatusCode!);

        }

    }
}