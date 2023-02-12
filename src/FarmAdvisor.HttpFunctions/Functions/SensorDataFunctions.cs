using System;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Linq;
using FarmAdvisor.Models.Models;
using FarmAdvisor.DataAccess.MSSQL.DataContext;
using FarmAdvisor.DataAccess.MSSQL.Functions.Crud;
using Newtonsoft.Json;
using FarmAdvisor.DataAccess.MSSQL.Functions.Interfaces;
//using AzureFunctions.Extensions.Swashbuckle.Attribute;
using FarmAdvisor.DataAccess.AzureTableStorage.services;
using FarmAdvisor.HttpFunctions.Services;

namespace FarmAdvisor.HttpFunctions.Functions
{
    public class SensorDataFunctions
    {
        private readonly ILogger<FarmFieldFunctions> _logger;
        private readonly ICrud _crud;

        public SensorDataFunctions(ILogger<FarmFieldFunctions> logger, ICrud crud)
        {
            _logger = logger;
            _crud = crud;
        }


        [FunctionName("GetAllSensordatas")]
        [OpenApiOperation(operationId: "GetAllSensordatas",
        tags: new[] { "GetAllSensordatas" },
        Summary = "Get all sensor datas",
        Description = "Get all sensor datas",
        Visibility = OpenApiVisibilityType.Important)]
        // [OpenApiParameter(name: "url",
        // In = ParameterLocation.Query,
        // Required = true,
        // Type = typeof(Uri),
        // Summary = "The URL to generate QR code",
        // Description = "The URL to generate QR code",
        // Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK,
        contentType: "json/application",
        bodyType: typeof(SensorData),
        Summary = "The fields",
        Description = "The fields")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest,
        Summary = "If no sensordata are found",
        Description = "If no sensordata are found")]
        public async Task<IActionResult> GetAllSensordatas(
             [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/farms/fields/sensorData")] HttpRequest req)
        {
            _logger.LogInformation("Executing {method}", nameof(GetAllSensordatas));
            var sensorDatas = await _crud.FindAll<SensorData>();

            return new OkObjectResult(sensorDatas);
        }

        
        [FunctionName("UpsertSensorDatas")]
        [OpenApiOperation(operationId: "UpsertSensorDatas",
        tags: new[] { "UpsertSensorDatas" },
        Summary = "add sensor datas",
        Description = "add sensor datas",
        Visibility = OpenApiVisibilityType.Important)]
        // [OpenApiParameter(name: "url",
        // In = ParameterLocation.Query,
        // Required = true,
        // Type = typeof(Uri),
        // Summary = "The URL to generate QR code",
        // Description = "The URL to generate QR code",
        // Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK,
        contentType: "json/application",
        bodyType: typeof(SensorData),
        Summary = "The sensor datas",
        Description = "The sensor datas")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest,
        Summary = "If no sensordata can be fetched",
        Description = "If no sensordata can be fetched")]
        public async Task<IActionResult> UpsertSensorDatas([HttpTrigger(AuthorizationLevel.Function, "post", Route = "users/farms/fields/sensorData")] HttpRequest req) {
            SensorApi api = new SensorApi();
            List<SensorData> sensorDatas = api.getReadings();
            _logger.LogInformation(sensorDatas[0].ToString());
            foreach (SensorData data in sensorDatas) {
                _logger.LogInformation(data.ToString());
                await _crud.Create<SensorData>(data);
            }
            
            return new OkObjectResult(sensorDatas);
        }

    }
}