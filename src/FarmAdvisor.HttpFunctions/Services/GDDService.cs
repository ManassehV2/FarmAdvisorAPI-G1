using FarmAdvisor.DataAccess.MSSQL.Functions.Interfaces;
using System;
using System.Collections.Generic;
using FarmAdvisor.Models.Models;
using System.Threading.Tasks;
using FarmAdvisor_HttpFunctions.Functions;
using FarmAdvisor.Business;
using System.Net.Http;
using System.Net.Http.Json;
using FarmAdvisor.DataAccess.MSSQL.DataContext;
using System.Linq;

namespace FarmAdvisor.HttpFunctions.Services {
    public class GDDService {
        private readonly ICrud crud;
        private readonly HttpClient _httpCLient;

        public GDDService(ICrud crud) {
            this.crud = crud;
            _httpCLient = new HttpClient();
        }

        public async Task<string> getForecastedGddIncreases(SensorModel sensor) {
            _httpCLient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36");

            // var context = new DatabaseContext(DatabaseContext.Options.DatabaseOptions);
            // var Field = context.Fields
            //                 .Where(u => u.FieldId == sensor.FieldId)
            //                 .FirstOrDefault();

            var forcast = await _httpCLient.GetAsync($"https://api.met.no/weatherapi/locationforecast/2.0/complete?lat={sensor.Lat.ToString()}&lon={sensor.Long.ToString()}&altitude=100");
            Console.WriteLine(forcast);
            var forcastedTemps = new List<int>();
            // populate forcasted temps

            var gddIncreaseByDay = new List<int>();
            // calculate gdd increase
            return String.Join(" ", gddIncreaseByDay);
        }

        public int getAccumulatedGddIncrease() {
            SensorApi sensorApi = new SensorApi();
            List<SensorData> datas = sensorApi.getReadings();

            var data = datas[0];
            int startPoint = data.startPoint;
            int maxTemp = -100;
            int minTemp = 100;
            foreach (string offset in data.sampleOffsets.Split(" ")) {
                int currTemp = startPoint + Int32.Parse(offset);
                maxTemp = Math.Max(maxTemp, currTemp);
                minTemp = Math.Min(minTemp, currTemp);
            }

            int gddIncrease = (maxTemp+minTemp)/2 + startPoint;
            return gddIncrease;
        }

    }
}