using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FarmAdvisor.DataAccess.MSSQL.DataContext;
using FarmAdvisor.DataAccess.MSSQL.Functions.Interfaces;
using FarmAdvisor.Models.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FarmAdvisor_HttpFunctions.Functionsw
{
    public class FieldEndpoint
    {
        public ICrud _crud;

        public FieldEndpoint(ICrud crud)
        {
            _crud = crud;
        }

        [FunctionName("AddFieldEndpoint")]
        public async Task<ActionResult<FarmModel>> AddFieldModel(
           [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
           ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            string ver = data?.farmId;
            Guid FarmId = new Guid(ver);
            string Name = data?.name;
            int Altitude = Convert.ToInt32(data?.altitude);
            string polygon = data?.polygon;

            FarmModel farm = await _crud.Find<FarmModel>(FarmId);
            if (farm == null)
            {
                return new NotFoundObjectResult("No farm found");
            }
            var field = new FieldModel { FarmId= FarmId, Name=Name, Alt=Altitude, Polygon= polygon};

            FieldModel responseMessage;
            try
            {
                responseMessage = await _crud.Create<FieldModel>(field);
            }
            catch (Exception ex)
            {
                return new NotFoundObjectResult(ex);
            }
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("GetFieldEndpoint")]
        public async Task<IActionResult> GetFieldModel(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "FieldApi/{id}")] HttpRequest req, Guid id,
           ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            try
            {
                using (var context = new DatabaseContext(DatabaseContext.Options.DatabaseOptions))
                {
                    var responseMessage = context.Fields
                            .Where(u => u.FieldId == id)
                            .Include("Sensors")
                            .FirstOrDefault();
                    return new OkObjectResult(responseMessage);

                }
            }
            catch (Exception ex)
            {
                return new NotFoundObjectResult(ex);
            }
        }
    }
}
