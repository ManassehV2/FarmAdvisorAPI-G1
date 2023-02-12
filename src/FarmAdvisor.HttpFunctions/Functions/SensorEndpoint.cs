using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FarmAdvisor.Models.Models;
using FarmAdvisor.DataAccess.MSSQL.Functions.Crud;
using FarmAdvisor.DataAccess.MSSQL.Functions.Interfaces;
using FarmAdvisor.DataAccess.MSSQL.DataContext;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Collections;
using static FarmAdvisor.Models.Models.SensorModel;

namespace FarmAdvisor_HttpFunctions.Functions
{
    public class SensorEndpoint
    {
        public ICrud _crud;
        public SensorEndpoint(ICrud crud)
        {
            _crud = crud;
        }
        [FunctionName("AddSensor")]
        public async Task<ActionResult<SensorModel>> AddSensor(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            string serialNumber = data?.serialNumber;

            SensorModel prevSensor;
            using (var context = new DatabaseContext(DatabaseContext.Options.DatabaseOptions))
            {
                prevSensor = await context.Sensors.FirstOrDefaultAsync(s => s.SerialNumber == serialNumber);

                if (prevSensor != null)
                {
                    return new ConflictObjectResult("Sensor  exists");
                }

                DateTime lastCommunication, cuttingDateTimeCalculated, lastForecastData;

                try
                {
                    lastCommunication = DateTime.Parse( data?.lastCommunication.ToString());
                    cuttingDateTimeCalculated = DateTime.Parse( data?.cuttingDateTimeCalculated.ToString());
                    lastForecastData = DateTime.Parse(data?.lastForecastData.ToString());
                }
                catch (FormatException ex)
                {
                    return new BadRequestObjectResult(ex);
                }

                int batteryStatus = data?.batteryStatus;
                int optimalGDD = data?.optimalGDD;
                
                double lat = data?.lat;
                double longt = data?.longt;
                StateEnum state = (StateEnum)StateEnum.Parse(typeof(StateEnum), data?.state.ToString());
                var sensor = new SensorModel { SensorId = Guid.NewGuid(), SerialNumber = serialNumber, LastCommunication = lastCommunication, BatteryStatus = batteryStatus, OptimalGDD = optimalGDD, CuttingDateTimeCalculated = cuttingDateTimeCalculated, LastForecastDate = lastCommunication, Lat = lat, Long = longt, State = state };

                //SensorModel responseMessage;

                try
                {
                    await context.Sensors.AddAsync(sensor);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return new NotFoundObjectResult(ex);
                }
                return new OkObjectResult(sensor);
            }
        }

        [FunctionName("GetSensor")]
        public async Task<IActionResult> GetSensor(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "SensorApi/{id}")] HttpRequest req, Guid id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            SensorModel responseMessage;

            try
            {
                responseMessage = await _crud.Find<SensorModel>(id);
            }
            catch (Exception ex)
            {
                return new NotFoundObjectResult(ex);
            }
            return new OkObjectResult(responseMessage);
        }


        [FunctionName("GetSensorsInField")]
        public async Task<IActionResult> GetSensorsInField(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "allSensors/{id}")] HttpRequest req, Guid id,
           ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            try
            {
                using (var context = new DatabaseContext(DatabaseContext.Options.DatabaseOptions))
                {
                    var responseMessage = context.Sensors
                            .Where(u => u.FieldId == id).ToList();

                    return new OkObjectResult(responseMessage);

                }
            }
            catch (Exception ex)
            {
                return new NotFoundObjectResult(ex);
            }
        }

        [FunctionName("EditSensor")]
        public async Task<ActionResult<UserModel>> EditSensor(
           [HttpTrigger(AuthorizationLevel.Function, "put", Route = "SensorApi/{id}")] HttpRequest req, Guid id,
           ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            var sensor = await _crud.Find<SensorModel>(id);

            sensor.SerialNumber = data?.serialNumber;
            sensor.OptimalGDD = data?.optimalGDD;
            sensor.LastForecastDate = data?.lastForecastDate;
            sensor.Lat = data?.lat;
            sensor.Long = data?.longt;
            sensor.Long = data?.longt;
            sensor.State = data?.state;

            SensorModel responseMessage;
            try
            {
                responseMessage = await _crud.Update<SensorModel>(id, sensor);
            }
            catch (Exception ex)
            {
                return new NotFoundObjectResult(ex);
            }
            return new OkObjectResult(responseMessage);
        }
    }
}
