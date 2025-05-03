using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using WeatherAggregator.ASPNet.Models;
using WeatherAggregator.Library.Entities;
using WeatherAggregator.Library.Interfaces;
using WeatherAggregator.Library.Interfaces.Entities;

namespace WeatherAggregator.ASPNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWeatherClientApiService _weatherService;

        private static readonly List<LocationModel> _locations = new()
    {
        new LocationModel { Name = "Linz", Latitude = 48.3069, Longitude = 14.2858 },
        new LocationModel { Name = "Berlin", Latitude = 52.52, Longitude = 13.405 },
        new LocationModel { Name = "New York", Latitude = 40.7128, Longitude = -74.0060 }
    };

        public HomeController(ILogger<HomeController> logger, IWeatherClientApiService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        [HttpPost]
        public async Task<IActionResult> GetWeatherInfo(LocationModel location)
        {
            Location loc = new Location(location.Latitude, location.Longitude, location.Name);

            Console.WriteLine("requesting weatherdata from {0}", location.Name);
            var weather = await _weatherService.GetWeatherAsync(loc);
            _locations.Where(x => x.Name == location.Name).ToList().ForEach(x => x.WeatherModels.Add(new WeatherModel(weather.Temperature, DateTime.Now)));
            return View("Index", _locations);
        }


        public IActionResult Index()
        {
            var weather = new List<WeatherModel>() { 
                new WeatherModel(24, DateTime.Parse("1.01.2000")), 
                new WeatherModel(24, DateTime.Parse("2.01.2000")),
                new WeatherModel(24, DateTime.Parse("3.01.2000")),
                new WeatherModel(24, DateTime.Parse("4.01.2000")),
            };
            var shanghai = new LocationModel("Shanghai", 31.221518, 121.544380);
            shanghai.WeatherModels = weather;
            _locations.Add(shanghai);
            return View(_locations);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
