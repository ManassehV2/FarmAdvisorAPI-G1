
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
    public sealed class TestSensorFunctions
    {
        readonly SensorEndpoint _sensorFunctions;
        public TestSensorFunctions()
        {
            var startup = new HttpFunctionStartup();
            var host = new HostBuilder()
                .ConfigureWebJobs(startup.Configure)
                .Build();

            _sensorFunctions = new SensorEndpoint(host.Services.GetRequiredService<ICrud>());
        }



        [Fact]
        public async Task AddSensorModelTest()
        {



            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            request.Method = "POST";
            var json = JsonConvert.SerializeObject(new SensorRequest
            {
               LastCommunication = "2/2/2022",
               CuttingDateTimeCalculated = "2/2/2022", 
               LastForecastData = "2/2/2022",
               Lat = (int)23.533534,
               Longt = (int)34.354324,
               BatteryStatus = 100,
               OptimalGDD = 450,
               State = "Working",
               FieldId = "D347A588-F3A1-462A-67D3-08DB0E48F632"

            });
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            request.Body = stream;

            request.ContentLength = stream.Length;
            request.ContentType = "application/json";
            var jsonResult = await _sensorFunctions.AddSensor(request);
            var result = (NotFoundObjectResult)jsonResult.Result!;
            Assert.Equal(404, result.StatusCode);



        }

        [Fact]
        public async Task GetUserNewTest()
        {
            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            request.Method = "GET";
            var jsonResult = await _sensorFunctions.GetSensor(request, new Guid());
            var result = (HttpStatusCode)jsonResult.GetType().GetProperty("StatusCode")!
                .GetValue(jsonResult, null)!;
            Assert.Equal(404, (int)result);

        }
    }
}
