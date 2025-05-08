using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WeatherAggregator.Library.Entities;
using WeatherAggregator.Library.Interfaces;
using WeatherAggregator.Library.Interfaces.Entities;
using static System.Net.WebRequestMethods;

namespace WeatherAggregator.Library.Service
{
    public class WeatherApiClientService: IWeatherClientApiService
    {
        private readonly HttpClient _httpClient;

        public WeatherApiClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
         public async Task<List<WeatherInfo>> CallMeteoApi(ILocation location, DateTime start, DateTime end)
        {
            string url = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude.ToString(CultureInfo.InvariantCulture)}&longitude={location.Longitude.ToString(CultureInfo.InvariantCulture)}&hourly=temperature_2m&start_date={start.ToString("yyyy-MM-dd")}&end_date={end.ToString("yyyy-MM-dd")}";
            var list = new List<WeatherInfo>();
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);

                Console.WriteLine(json);
                var hourly = json["hourly"];
                if (hourly != null)
                {
                    var timeArray = hourly["time"]?.ToObject<List<string>>();
                    var tempArray = hourly["temperature_2m"]?.ToObject<List<double>>();

                    if (timeArray != null && tempArray != null && timeArray.Count == tempArray.Count)
                    {
                        for (int i = 0; i < timeArray.Count; i++)
                        {
                            list.Add(new WeatherInfo
                            {
                                Time = DateTime.Parse(timeArray[i]),
                                Temperature = tempArray[i]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Weather fetch failed: {ex.Message}");
            }

            return list;
        }

         public async Task<List<WeatherInfo>> CallWeatherApi(ILocation location, DateTime start, DateTime end)
        {
            var days = end - start;
            string url = $"https://api.weatherapi.com/v1/forecast.json?q={location.Latitude},{location.Longitude}&days={days.Days}";
            var list = new List<WeatherInfo>();
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);

                var hours = json["forecast"]?["forecastday"]?[0]?["hour"];
                if (hours != null)
                {
                    foreach (var hour in hours)
                    {
                        var time = hour["time"]?.ToString();
                        var temp = hour["temp_c"]?.ToObject<double>();

                        if (time != null && temp != null)
                        {
                            list.Add(new WeatherInfo
                            {
                                Time = DateTime.Parse(time),
                                Temperature = temp.Value
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Weather fetch failed: {ex.Message}");
            }

            return list;
        }

        public async Task<List<WeatherInfo?>> Forecast(ILocation location, DateTime start, DateTime end)
        {
            return null;
        }
        public async Task<WeatherInfo?> GetWeatherAsync(ILocation location)
        {
            string url = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&current_weather=true";

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
                        Time = current.Value<DateTime>("time")
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
