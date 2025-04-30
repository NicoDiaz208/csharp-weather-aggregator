using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeatherAggregator.ASPNet.Models;
using WeatherAggregator.Library.Interfaces;
using WeatherAggregator.Library.Interfaces.Entities;

namespace WeatherAggregator.ASPNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWeatherService _weatherService;
        private static List<string> sidebarItems = new List<string> { "Start", "Über uns", "Kontakt" };

        public HomeController(ILogger<HomeController> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        public async Task<IActionResult> getWeatherInfo(ILocation location)
        {
            return View(await _weatherService.GetWeatherAsync(location));
        }


        public IActionResult Index()
        {
            ViewBag.Items = sidebarItems;
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
