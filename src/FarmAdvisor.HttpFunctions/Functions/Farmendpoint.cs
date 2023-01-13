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

            var farm = new FarmModel { Name = name, Postcode = postcode, City = city, Country = country };


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

        [FunctionName("FarmendpointNew")]
        public async Task<IActionResult> GetFarmNew(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Farmendpoint/{id}")] HttpRequest req, Guid id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            FarmModel responseMessage;

            try
            {
                responseMessage = await _crud.Find<FarmModel>(id);
            }
            catch (Exception ex)
            {
                return new NotFoundObjectResult(ex);
            }
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("FarmendpointDel")]
        public async Task<IActionResult> DelFarm(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "Farmendpoint/{id}")] HttpRequest req, Guid id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            FarmModel responseMessage;

            try
            {
                responseMessage = await _crud.Delete<FarmModel>(id);
            }
            catch (Exception ex)
            {
                return new NotFoundObjectResult(ex);
            }
            return new OkObjectResult(responseMessage);
        }
        [FunctionName("FarmendpointEdit")]

        public async Task<ActionResult<FarmModel>> EditFarm([HttpTrigger(AuthorizationLevel.Function, "put", Route = "Farmendpoint/{id}")] HttpRequest req, Guid id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            var farm = await _crud.Find<FarmModel>(id);

            farm.Name = data?.name;
            farm.Postcode = data?.postcode;
            farm.City= data?.city;
            farm.Country= data?.country;


            FarmModel responseMessage;
            try
            {
                responseMessage = await _crud.Update<FarmModel>(id, farm);
            }
            catch (Exception ex)
            {
                return new NotFoundObjectResult(ex);
            }
            return new OkObjectResult(responseMessage);
        }

    }

    }
