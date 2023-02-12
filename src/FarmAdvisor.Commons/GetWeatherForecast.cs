using FarmAdvisor.DataAccess.MSSQL.DataContext;
using FarmAdvisor.Models.Models;
using System.Net.Http.Json;
namespace FarmAdvisor.Business
{

    public class GetWeatherForecast:IDisposable
    {
        private readonly HttpClient _httpCLient;
  

        public GetWeatherForecast()
        {
            _httpCLient = new HttpClient();
        }

        public void Dispose()
        {
            ((IDisposable)_httpCLient).Dispose();
        }

        public async Task<WeatherForecast?> GetLocationForecast(SensorModel sensor)
        {
            _httpCLient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36");
            try
            {
                using (var context = new DatabaseContext(DatabaseContext.Options.DatabaseOptions))
                {
                    var Field = context.Fields
                            .Where(u => u.FieldId == sensor.FieldId)
                            .FirstOrDefault();


                    WeatherForecast response = await _httpCLient.GetFromJsonAsync<WeatherForecast>($"https://api.met.no/weatherapi/locationforecast/2.0/complete?lat={sensor.Lat.ToString()}&lon={sensor.Long.ToString()}&altitude={Field.Alt.ToString()}");
                    return response;
                }
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }


    }
}
