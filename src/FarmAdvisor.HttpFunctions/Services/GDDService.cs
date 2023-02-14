using FarmAdvisor.DataAccess.MSSQL.Functions.Interfaces;
using System;
using System.Collections.Generic;
using FarmAdvisor.Models.Models;
using System.Threading.Tasks;
using FarmAdvisor_HttpFunctions.Functions;
using System.Net.Http;
using System.Net.Http.Json;
using FarmAdvisor.DataAccess.MSSQL.DataContext;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

            var response = await _httpCLient.GetAsync($"https://api.met.no/weatherapi/locationforecast/2.0/complete?lat={sensor.Lat.ToString()}&lon={sensor.Long.ToString()}&altitude=100");
            var jsonString = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonString);
            
            var weatherData = json["properties"]["timeseries"];
            var forcastedTemps = new List<int>();
            foreach (var weather in weatherData) {
                forcastedTemps.Add((int)weather["data"]["instant"]["details"]["air_temperature"]);
            }
            // Console.WriteLine(weather);
            var gddIncreaseByDay = new List<int>();
            // calculate gdd increase
            int hour = 0;
            int day_max = -100;
            int day_min = 100;
            int curr_total = 0;
            foreach (int temp in forcastedTemps) {
                hour += 1;
                curr_total += temp;
                day_min = Math.Min(day_min, temp);
                day_max = Math.Max(day_max, temp);
                if (hour == 10) {
                    int gddIncrease = ((day_max+day_min)/2) - 10;
                    hour = 0;
                    day_max = -100;
                    day_min = 100;
                    curr_total = 0;
                    if (gddIncrease < 0) {
                        gddIncrease = 0;
                    }
                    gddIncreaseByDay.Add(gddIncrease);
                }
                
            }
            var forcastedIncreaseString = String.Join(" ", gddIncreaseByDay);
            return forcastedIncreaseString;
        }

        public static int getAccumulatedGddIncrease() {
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

            int gddIncrease = (maxTemp+minTemp)/2 - 10;
            return gddIncrease;
        }

    }
}