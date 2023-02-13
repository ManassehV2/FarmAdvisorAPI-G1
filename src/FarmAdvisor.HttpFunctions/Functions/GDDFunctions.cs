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
// using AzureFunctions.Extensions.Swashbuckle.Attribute;
using FarmAdvisor.DataAccess.AzureTableStorage.services;
using FarmAdvisor.HttpFunctions.Services;

namespace FarmAdvisor.HttpFunctions.Functions
{
    public class GDDFunctions
    {
        private readonly ILogger<FarmFieldFunctions> _logger;
        private readonly ICrud _crud;

        public GDDFunctions(ILogger<FarmFieldFunctions> logger, ICrud crud)
        {
            _logger = logger;
            _crud = crud;
        }

        
        [FunctionName("ResetGDDByDate")]
        [OpenApiOperation(operationId: "ResetGDDByDate",
        tags: new[] { "ResetGDDByDate" },
        Summary = "add sensor datas",
        Description = "add sensor datas",
        Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK,
        contentType: "json/application",
        bodyType: typeof(SensorData),
        Summary = "The sensor datas",
        Description = "The sensor datas")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest,
        Summary = "If no sensordata can be fetched",
        Description = "If no sensordata can be fetched")]
        public async Task<IActionResult> ResetGDDByDate([HttpTrigger(AuthorizationLevel.Function, "update", Route = "users/farms/fields/reset/{id}")] HttpRequest req, string id) {
            FieldModel field = await _crud.Find<FieldModel>(new Guid(id));

            if (field == null) {
                return new NotFoundObjectResult(field);
            }
            GDDService gDDService = new GDDService(_crud);
            field.accumulatedGdd = 0;
            SensorModel currSensor = field.Sensors.OfType<SensorModel>().FirstOrDefault();

            if (currSensor == null) {
                return new NotFoundObjectResult("no sensor registered");
            }

            field.forecastedGdd = await gDDService.getForecastedGddIncreases(currSensor);
            return new OkObjectResult(field);
        }

        [FunctionName("UpdateGDD")]
        public async Task<IActionResult> UpdateGDD([HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/farms/fields/update")] HttpRequest req) { 
            GDDService service = new GDDService(_crud);
            SensorModel sensor = new SensorModel();
            sensor.Lat = 100;
            sensor.Long = 45;
            var forcast = await service.getForecastedGddIncreases(sensor);

            return new OkObjectResult(forcast);
         }

    }
}