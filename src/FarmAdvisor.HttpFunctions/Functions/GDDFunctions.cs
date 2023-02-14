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
using System.Collections.ObjectModel;

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

            try {
                List<FieldModel> fields = await _crud.FindAll<FieldModel>();
                if (true) {
                    var user = new UserModel();
                    user.AuthId = "01";
                    user.Email = "email";
                    user.Name = "myUser";
                    user.Phone = "32";
                    user = await _crud.Create<UserModel>(user);

                    FarmModel farm = new FarmModel();
                    farm.UserId = user.UserID;
                    farm.City = "AA";
                    farm.Country = "Eth";
                    farm.Name = "myFarm";
                    farm.Postcode = "10";
                    farm = await _crud.Create<FarmModel>(farm);

                    FieldModel field = new FieldModel();
                    field.FarmId = farm.FarmId;
                    field.accumulatedGdd = 0;
                    field.forecastedGdd = " ";
                    field.Alt = 100;
                    field.Name = "myfield";
                    field.Polygon = "whatPolygon";
                    field = await _crud.Create<FieldModel>(field);

                    fields.Add(field);
                    SensorModel sensor = new SensorModel();
                    Console.WriteLine("0 fields found");
                    sensor.FieldId = field.FieldId;
                    sensor.BatteryStatus = 10;
                    sensor.OptimalGDD = 500;
                    sensor.Lat = 5;
                    sensor.Long = 10;
                    await _crud.Create<SensorModel>(sensor);
                }
                GDDService gDDService = new GDDService(_crud);
                var context = new DatabaseContext(DatabaseContext.Options.DatabaseOptions);
                foreach (var field in fields) {
                    var allsensors = context.Sensors.Where(s => s.FieldId == field.FieldId).ToList();
                    if (allsensors.Capacity == 0) {
                        continue;
                    }
                    if (allsensors.Capacity <= 0) {
                        continue;
                    }
                    var sensor = allsensors[0];
                    var forcastedGddIncrease = await gDDService.getForecastedGddIncreases(sensor);
                    field.forecastedGdd = forcastedGddIncrease;

                    Console.WriteLine(field.forecastedGdd);
                    field.accumulatedGdd += gDDService.getAccumulatedGddIncrease();
                    Console.WriteLine(field.accumulatedGdd);
                    await _crud.Update<FieldModel>(field.FieldId, field);
                }

                return new OkObjectResult("updated GDD");
                
            } catch {
                _logger.LogInformation("Error while updating field GDD");
                return new UnprocessableEntityObjectResult("unable to update GDD");
            }

         }


        [FunctionName("UpdateFieldsGDD")]
        public async Task updateFarmsGDD([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer) {

            try {
                List<FieldModel> fields = await _crud.FindAll<FieldModel>();

                GDDService gDDService = new GDDService(_crud);
                var context = new DatabaseContext(DatabaseContext.Options.DatabaseOptions);
                foreach (var field in fields) {
                    var allsensors = context.Sensors.Where(s => s.FieldId == field.FieldId).ToList();
                    if (allsensors.Capacity == 0) {
                        continue;
                    }
                    if (allsensors.Capacity <= 0) {
                        continue;
                    }
                    var sensor = allsensors[0];
                    var forcastedGddIncrease = await gDDService.getForecastedGddIncreases(sensor);
                    field.forecastedGdd = forcastedGddIncrease;

                    field.accumulatedGdd += gDDService.getAccumulatedGddIncrease();
                    await _crud.Update<FieldModel>(field.FieldId, field);
                }
                
            } catch {
                _logger.LogInformation("Error while updating field GDD");
            }
            
        }

    }
}