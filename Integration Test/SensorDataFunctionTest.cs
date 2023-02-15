
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
using FarmAdvisor.HttpFunctions.Functions;

namespace FarmAdvisor.IntegrationTest
{
    public sealed class TestSensorDataFunctions
    {
        readonly SensorDataFunctions _sensorDataFunctions;
        public TestSensorDataFunctions()
        {
            var startup = new HttpFunctionStartup();
            var host = new HostBuilder()
                .ConfigureWebJobs(startup.Configure)
                .Build();

            _sensorDataFunctions = new SensorDataFunctions(host.Services.GetRequiredService<ICrud>());
        }



        [Fact]
        public async Task AddSensorModelTest()
        {



            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            request.Method = "POST";
            var json = JsonConvert.SerializeObject(new SensorData
            {
                //SensorDataId= Guid.NewGuid(),
                serialNum = "231345tlewrktjf2354",
                type = "test",
                measurementPeriodBase = 0,
                batteryStatus = true,
                nextTransmissionAt = "2/2/2023",
                signal = 1,
                timeStamp = "2/2/2023",
                startPoint = 23,
                sampleOffsets = "test",
                SensorId = Guid.NewGuid(),
                
            });
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            request.Body = stream;

            request.ContentLength = stream.Length;
            request.ContentType = "application/json";
            var jsonResult = await _sensorDataFunctions.UpsertSensorDatas(request);
            var result = (NotFoundObjectResult)jsonResult;
            Assert.Equal(404, result.StatusCode);



        }

        [Fact]
        public async Task GetAllSensordatasTest()
        {
            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            request.Method = "GET";
            var jsonResult = await _sensorDataFunctions.GetAllSensordatas(request);
            var result = (HttpStatusCode)jsonResult.GetType().GetProperty("StatusCode")!
                .GetValue(jsonResult, null)!;
            Assert.Equal(200, (int)result);

        }
    }
}
