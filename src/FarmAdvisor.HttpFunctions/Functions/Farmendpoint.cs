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

namespace FarmAdvisor_HttpFunctions.Functions
{
    public class Farmendpoint
    {
        public ICrud _crud;
        public Farmendpoint(ICrud crud)
        {
            _crud = crud;
        }
        [FunctionName("Farmendpoint")]
        public async Task<ActionResult<FarmModel>> AddFarm(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            //string name = req.Query["name"];
            string name = data?.name;

            FarmModel prevFarm;
            using (var context = new DatabaseContext(DatabaseContext.Options.DatabaseOptions))
            {
                prevFarm = await context.Farms.FirstOrDefaultAsync(s => s.Name == name);
            }
            if (prevFarm != null)
            {
                return new ConflictObjectResult("Farm Exist");
            }
            string postcode = data?.postcode;
            string city = data?.city;
            string country = data?.country;

        var farm = new FarmModel {Name = name, Postcode = postcode, City = city , Country = country};


        FarmModel responseMessage;
            try
            {
                responseMessage = await _crud.Create<FarmModel>(farm);
            }
            catch (Exception ex)
            {
                return new NotFoundObjectResult(ex);
            }
            return new OkObjectResult(responseMessage);
        }

        string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
