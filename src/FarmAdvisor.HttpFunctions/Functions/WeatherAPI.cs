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
using FarmAdvisor.DataAccess.MSSQL.DataContext;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq;
using FarmAdvisor.Business;
using System.Collections.Generic;
using FarmAdvisor.DataAccess.MSSQL.Functions.Interfaces;

namespace FarmAdvisor_HttpFunctions.Functions
{
    public class WeatherAPI
    {

        private readonly ICrud _crud;
        public WeatherAPI(ICrud crud)
        {
            _crud = crud;
        }
        [FunctionName("WeatherAPI")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function,"post", Route = "api/getforecast")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            

            string fieldID = data?.FieldID;
            Guid id = new Guid(fieldID);
            var field = await _crud.Find<FieldModel>(id);
            log.LogInformation($"{field}");
           
            if (field != null)
                {
                    using (var context = new DatabaseContext(DatabaseContext.Options.DatabaseOptions))
                    {
                        var allsensors = context.Sensors.Where(s => s.FieldId ==id).ToList();
                    log.LogInformation(allsensors.ToString());

                        var result = new List<WeatherForecast>();

                        foreach(var sensor in allsensors)
                        {
                            using var forecast = new GetWeatherForecast();
                            {
                               result.Add( await forecast.GetLocationForecast(sensor));
                            }
                        }

                    return new OkObjectResult(result);
                    }
                }
                else
                {
                log.LogInformation("notfound");
                    return new NotFoundObjectResult(data);
                }
            }
    }
}
