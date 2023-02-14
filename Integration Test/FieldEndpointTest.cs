
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
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using Integration_Test;
using Newtonsoft.Json;
using System.Text;

namespace FarmAdvisor.IntegrationTest
{
    public sealed class TestFieldFunctions
    {
        readonly FieldEndpoint _fieldFunctions;
        public TestFieldFunctions()
        {
            var startup = new HttpFunctionStartup();
            var host = new HostBuilder()
                .ConfigureWebJobs(startup.Configure)
                .Build();

            _fieldFunctions = new FieldEndpoint(host.Services.GetRequiredService<ICrud>());
        }



        [Fact]
        public async Task AddFieldModelTest()
        {
            
            

            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            request.Method = "POST";
            var json = JsonConvert.SerializeObject(new FieldRequest
            {
                name= "name",
                altitude = "124",
                polygon = "rectangle",
                farmId = "49f5eb3f-3d97-439f-caaa-08db0e48ae8c"
            });
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            request.Body = stream;

            request.ContentLength = stream.Length;
            request.ContentType = "application/json"; 
            var jsonResult = await _fieldFunctions.AddFieldModel(request);
            var result = (OkObjectResult)jsonResult.Result!;
            Assert.Equal(200, result.StatusCode);



        }
        
        [Fact]
        public async Task GetUserNewTest()
        {
            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            request.Method = "GET";
            var jsonResult = await _fieldFunctions.GetFieldModel(request, new Guid());
            var result = (HttpStatusCode)jsonResult.GetType().GetProperty("StatusCode")!
                .GetValue(jsonResult, null)!;
            Assert.Equal(200, (int)result);

        }
    }
}