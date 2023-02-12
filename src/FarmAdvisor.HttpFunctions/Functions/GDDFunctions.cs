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
        public async Task<IActionResult> ResetGDDByDate([HttpTrigger(AuthorizationLevel.Function, "update", Route = "users/farms/fields/sensors/Gdd/reset/{id}")] HttpRequest req) {
            
        }

    }
}