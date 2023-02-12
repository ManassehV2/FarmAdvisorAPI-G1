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
    public class FarmEndpoint
    {
        public ICrud _crud;

        public FarmEndpoint(ICrud crud)
        {
            _crud = crud;
        }

        [FunctionName("AddFarmEndpoint")]
        public async Task<ActionResult<FarmModel>> AddFarmModel(
           [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
           ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
           


            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string? userId = data?.userId;

            if (userId is null)
            {
                return new NotFoundObjectResult("No user Id provided");
            }
            string Name = data?.name;
            string PostCode = data?.postcode;
            string City = data?.city;
            string Country = data?.country;

            Guid UserId = new Guid(userId);
            UserModel? user = await _crud.Find<UserModel>(UserId);

            if (user is null)
            {
                return new NotFoundObjectResult("No user Id provided");
            }
            var farm = new FarmModel { Name = Name, Postcode = PostCode, City = City, Country = Country, UserId = user.UserID };

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

        [FunctionName("GetFarmEndpoint")]
        public async Task<IActionResult> GetFarmModel(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "FarmApi/{id}")] HttpRequest req, Guid id,
           ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            try
            {
                using (var context = new DatabaseContext(DatabaseContext.Options.DatabaseOptions))
                {
                    var responseMessage = context.Farms
                            .Where(u => u.FarmId == id)
                            .Include("Fields")
                            .Include("Notifications")
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
