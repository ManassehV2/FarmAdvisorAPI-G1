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
using FarmAdvisor.DataAccess.MSSQL.Functions.Interfaces;
using FarmAdvisor.DataAccess.MSSQL.DataContext;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FarmAdvisor_HttpFunctions.Functions
{
    public class UserApi
    {
        public ICrud _crud;
        public UserApi(ICrud crud)
        {
            _crud = crud;
        }

        [FunctionName("AddUserApi")]
        public async Task<ActionResult<UserModel>> AddUser(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
           


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            string Phone = data?.Phone;
            string name = data?.Name;
            string email = data?.Email;
            string authId = data?.AuthId;

            var user = new UserModel { Name = name, Phone = Phone, Email = email, AuthId = authId };

            UserModel responseMessage;
            try
            {
                responseMessage = await _crud.Create<UserModel>(user);
            }
            catch (Exception ex)
            {
                return new NotFoundObjectResult(ex);
            }
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("GetUserApiNew")]
        public  async Task<IActionResult> GetUserNew(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "UserApi/{id}")] HttpRequest req, string id)
            
        {


            try
            {
                await using (var context = new DatabaseContext(DatabaseContext.Options.DatabaseOptions))
                {
                var responseMessage = context.Users
                        .Where(u => u.AuthId == id)
                        .Include("Farms")
                        .FirstOrDefault();
                return new OkObjectResult(responseMessage);

                }
            }
            catch (Exception ex)
            {
                return new NotFoundObjectResult(ex);
            }
        }
        [FunctionName("UserApiEdit")]
        public async Task<ActionResult<UserModel>> EditUser(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "UserApi/{id}")] HttpRequest req, string id
            )
        {



            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            //var user = await _crud.Find<UserModel>(id);
            using (var context = new DatabaseContext(DatabaseContext.Options.DatabaseOptions))
            {

                var user = context.Users
                        .Where(u => u.AuthId == id)
                        .FirstOrDefault();
                user.Name = data?.name;
                user.Phone = data?.phone;
                user.Email = data?.email;
                user.AuthId = data?.authId;




                UserModel responseMessage;
                try
                {
                    responseMessage = await _crud.Update<UserModel>(user.UserID, user);
                }
                catch (Exception ex)
                {
                    return new NotFoundObjectResult(ex);
                }
                return new OkObjectResult(responseMessage);

            }
        }
        [FunctionName("UserApiDelete")]
        public async Task<ActionResult<UserModel>> DeleteUser(
           [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "UserApi/{id}")] HttpRequest req, Guid id,
           ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            bool status;
            try
            {
                status = await _crud.Delete<UserModel>(id);
            }
            catch (Exception ex)
            {
                return new NotFoundObjectResult(ex);
            }
            if (!status){
                return new NotFoundObjectResult("Not Succeeded");
            }
            return new OkObjectResult("Deleted succesfully");
        }

    }

}
