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
        private readonly IWeatherService _weatherService;

        private static List<string> sidebarItems = new List<string> { "Start", "Über uns", "Kontakt" };

        private static readonly List<LocationModel> _locations = new()
    {
        new LocationModel { Name = "Linz", Latitude = 48.3069, Longitude = 14.2858 },
        new LocationModel { Name = "Berlin", Latitude = 52.52, Longitude = 13.405 },
        new LocationModel { Name = "New York", Latitude = 40.7128, Longitude = -74.0060 }
    };
        private IWeatherInfo weatherInfo = new WeatherInfo();

        public HomeController(ILogger<HomeController> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        [HttpPost]
        public async Task<IActionResult> getWeatherInfo(LocationModel location)
        {
            Location loc = new Location(location.Latitude, location.Longitude, location.Name);
            ViewBag.SelectedLocation = location.Name;
            Console.WriteLine("requesting weatherdata from {0}", location.Name);
            return View("Index", await _weatherService.GetWeatherAsync(loc));
        }


        public IActionResult Index()
        {
            ViewBag.Items = _locations;
               
            return View();
        }

        [HttpPost]
        public IActionResult AddItem(string newItem)
        {
            if (!string.IsNullOrWhiteSpace(newItem))
                sidebarItems.Add(newItem);

            return RedirectToAction("Index");
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
