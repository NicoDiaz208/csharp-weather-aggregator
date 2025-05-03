using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAggregator.Library.Database;
using WeatherAggregator.Library.Entities;
using WeatherAggregator.Library.Interfaces;

namespace WeatherAggregator.Library.Service
{
    public class DataCSVImporter
    {
        private IWeatherRepository _weatherRepository;
        public DataCSVImporter(IWeatherRepository repository) { 
            _weatherRepository = repository;
        }
        public async Task ImportAndJoin(String filePath, Location location)
        {
            var weatherList = new List<WeatherInfo>();
            var lines = File.ReadAllLines(filePath);

            if (lines.Length <= 1)
            {
                Console.WriteLine("No data could be found!");
                return;
            }

            var header = lines[0].Split(',');

            // Find indexes of 'time' and 'temp'
            int timeIndex = Array.IndexOf(header, "time");
            int tempIndex = Array.IndexOf(header, "temp");

            if (timeIndex == -1 || tempIndex == -1)
                throw new InvalidOperationException("CSV missing 'time' or 'temp' columns");

            for (int i = 1; i < lines.Length; i++)
            {
                var parts = lines[i].Split(',');

                if (parts.Length <= Math.Max(timeIndex, tempIndex))
                    continue;

                // Parse DateTime
                if (!DateTime.TryParse(parts[timeIndex], out var time))
                    continue;

                var parsedTime = DateTime.SpecifyKind(time, DateTimeKind.Utc);

                if (!double.TryParse(parts[tempIndex], NumberStyles.Any, CultureInfo.InvariantCulture, out var temperature))
                    continue; // skip if invalid temperature

                weatherList.Add(new WeatherInfo
                {
                    Location = location,
                    LocationId = location.Id,
                    Time = parsedTime,
                    Temperature = temperature
                });

                Console.WriteLine($"{location.Name}, {time}, {temperature}");
            }

            await _weatherRepository.AddAllWeatherAsync(weatherList);
        }
    }
}
