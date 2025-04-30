using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WeatherAggregator.Library.Entities;
using WeatherAggregator.Library.Interfaces;

namespace WeatherAggregator.Library.Service
{
    public class WeatherService: IWeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherInfo?> GetWeatherAsync(double latitude, double longitude)
        {
            string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current_weather=true";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);
                var current = json["current_weather"];

                if (current != null)
                {
                    return new WeatherInfo
                    {
                        Temperature = current.Value<double>("temperature"),
                        Time = current.Value<string>("time")
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Weather fetch failed: {ex.Message}");
            }

            return null;
        }
    }
}
