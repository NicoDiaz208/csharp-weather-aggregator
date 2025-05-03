using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAggregator.Library.Database;
using WeatherAggregator.Library.Entities;
using WeatherAggregator.Library.Service;

namespace WeatherAggregator.Library.Background
{
    public class WeatherPollingService: BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly WeatherApiClientService _weatherService;
        private readonly List<Location> _locations = new List<Location>();
        public WeatherPollingService(IServiceProvider provider)
        {
            _provider = provider;
            _weatherService = new WeatherApiClientService(new HttpClient());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _provider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();

                foreach (var location in _locations)
                {
                    var weather = await _weatherService.GetWeatherAsync(location);
                    if (weather != null)
                    {
                        var dbLocation = db.Locations.FirstOrDefault(l => l.Name == location.Name);
                        if (dbLocation == null)
                        {
                            dbLocation = new Location(location.Latitude, location.Longitude, location.Name);
                            db.Locations.Add(dbLocation);
                            await db.SaveChangesAsync();
                        }

                        db.WeatherInfos.Add(new WeatherInfo
                        {
                            Temperature = weather.Temperature,
                            Time = weather.Time,
                            LocationId = dbLocation.Id
                        });
                        await db.SaveChangesAsync();
                    }
                }

                // Sleep 1 hour
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
        public void AddLocation(Location location)
        {
            _locations.Add(location);
        }

        public void RemoveLocation(String location)
        {
            _locations.RemoveAll(x => x.Name == location);
        }
    }
}
