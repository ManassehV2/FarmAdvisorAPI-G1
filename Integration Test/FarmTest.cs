
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
using FarmAdvisor_HttpFunctions.Functionsw;
using Microsoft.AspNetCore.Http.Features;

namespace FarmAdvisor.IntegrationTest
{
    public sealed class TestFarmFunctions
    {
        readonly FarmEndpoint _farmFunctions;
        public TestFarmFunctions()
        {
            var startup = new HttpFunctionStartup();
            var host = new HostBuilder()
                .ConfigureWebJobs(startup.Configure)
                .Build();

            _farmFunctions = new FarmEndpoint(host.Services.GetRequiredService<ICrud>());
        }



        [Fact]
        public async Task AddFarmModelTest()
        {
            var httpContext= new DefaultHttpContext();
            var request = httpContext.Request;
            var jsonResult = await _farmFunctions.AddFarmModel(request);
            var result = (NotFoundObjectResult)jsonResult.Result!;
            Assert.Equal(404, result.StatusCode); 


            
        }
        public readonly ILogger log;
        [Fact]
        public async Task GetUserNewTest()
        {
            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            var jsonResult = await _farmFunctions.GetFarmModel(request, new Guid());
            var result = (HttpStatusCode)jsonResult.GetType().GetProperty("StatusCode")
                .GetValue(jsonResult, null);
            Assert.Equal(200, (int)result);

        }
    }   
}