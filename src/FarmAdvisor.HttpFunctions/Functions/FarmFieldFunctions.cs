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
using AzureFunctions.Extensions.Swashbuckle.Attribute;

namespace FarmAdvisor.HttpFunctions.Functions
{
    public class FarmFieldFunctions
    {
        private readonly ILogger<FarmFieldFunctions> _logger;
        private readonly ICrud _crud;

        public FarmFieldFunctions(ILogger<FarmFieldFunctions> logger, ICrud crud)
        {
            _logger = logger;
            _crud = crud;
        }


        [FunctionName("GetAllFarmFields")]
        [OpenApiOperation(operationId: "GetAllFarmFields",
        tags: new[] { "GetAllFarmFields" },
        Summary = "Get all farm fields",
        Description = "Get all farm fields",
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
        bodyType: typeof(FarmFieldModel),
        Summary = "The fields",
        Description = "The fields")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest,
        Summary = "If no fields are found",
        Description = "If no fields are found")]
        public async Task<IActionResult> GetAllFarmFields(
             [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/farms/fields")] HttpRequest req)
        {
            _logger.LogInformation("Executing {method}", nameof(GetAllFarmFields));
            var fields = await _crud.FindAll<FarmFieldModel>();

            return new OkObjectResult(fields);
        }

        
        [FunctionName("GetFarmField")]
        public async Task<IActionResult> GetFarmField([HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/farms/fields/{id}")] HttpRequest req, string id) {
            _logger.LogInformation("Executing {method}", nameof(GetFarmField));
            try {
                var result = await _crud.Find<FarmFieldModel>(new Guid(id));
                return new OkObjectResult(result);
            } catch {
                return new NotFoundResult();
            }
        }

        [FunctionName("CreateFarmField")]
        public async Task<IActionResult> CreateFarmField([HttpTrigger(AuthorizationLevel.Function, "post", Route = "users/farms/fields")] HttpRequest req) {
            
            string reqBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<FarmFieldModel>(reqBody);

            _logger.LogInformation("Executing {method}", nameof(CreateFarmField));
            var newFarmField = new FarmFieldModel() {Name=input.Name, FarmId=input.FarmId, Altitude=input.Altitude, Polygon=input.Polygon};
            await _crud.Create<FarmFieldModel>(newFarmField);
            return new OkObjectResult(newFarmField);
        }

        [FunctionName("UpdateFarmField")]
        public async Task<IActionResult> UpdateFarmField([HttpTrigger(AuthorizationLevel.Function, "update", Route = "users/farms/fields/{id}")] HttpRequest request, string id) {
            var Id = new Guid(id);
            try {
                var field = await _crud.Find<FarmFieldModel>(Id);
            } catch (KeyNotFoundException) {
                return new NotFoundResult();
            }

            _logger.LogInformation("Executing {method}", nameof(UpdateFarmField));
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<FarmFieldModel>(requestBody);
            
            await _crud.Update<FarmFieldModel>(Id, updated);
            return new OkObjectResult(updated);
        }

        [FunctionName("DeleteFarmField")]
        public async Task<IActionResult> DeleteFarmField([HttpTrigger(AuthorizationLevel.Function, "delete", Route ="users/farms/fields/{id}")] HttpRequest request, string id) {

            var Id = new Guid(id);
            try {
                var field = await _crud.Find<FarmFieldModel>(Id);
            } catch (KeyNotFoundException) {
                return new NotFoundResult();
            }

            _logger.LogInformation("Executing {method}", nameof(DeleteFarmField));
            await _crud.Delete<FarmFieldModel>(Id);
            return new OkResult();
        }

    }
}