using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Microsoft.Identity.Client;

namespace FarmAdvisor.DataAccess.MSSQL.DataContext
{
    public class AppConfiguration
    {
        /*     public AppConfiguration()
             {
                 var ConfigBuilder = new ConfigurationBuilder();
                 var path = Path.Combine(Directory.GetCurrentDirectory(), "local.settings.json");
                 ConfigBuilder.AddJsonFile(path, false);
                 var root = ConfigBuilder.Build();
                 var LocalSettings = root.GetSection("ConnectionStrings:DefaultConnection");
                 if (LocalSettings != null)
                 {
                     SqlConnectionString = LocalSettings.Value!;
                 }
                 else
                 {
                     throw new InvalidDataException("must provide a connection string for mssql db");
                 }

             }
        */

        public String SqlConnectionString = "Data Source=DESKTOP-PSSR580;Initial Catalog=farm_api_try1;Integrated Security=True";

    }
}
