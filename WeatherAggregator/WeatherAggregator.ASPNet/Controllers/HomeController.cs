using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeatherAggregator.ASPNet.Models;
using WeatherAggregator.Library.Entities;
using WeatherAggregator.Library.Interfaces;

namespace WeatherAggregator.ASPNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWeatherClientApiService _weatherService;
        private readonly IWeatherRepository _weatherRepository;

        private HomeViewModel _homeViewModel = new HomeViewModel();

        public HomeController(ILogger<HomeController> logger, IWeatherClientApiService weatherService, IWeatherRepository weatherRepository)
        {
            _logger = logger;
            _weatherService = weatherService;
            _weatherRepository = weatherRepository;
        }

        [HttpPost]
        public async Task<IActionResult> GetWeatherInfo(LocationModel location)
        {
            _homeViewModel.CurrentLocation = location;
            var weatherdata = await _weatherRepository.GetWeatherInfoFromLocation(location.Id);
            if (weatherdata != null)
            {
                _homeViewModel.WeatherModels = weatherdata.Select(x => new WeatherModel(x.Temperature, x.Time)).ToList();
            }
            else { Console.WriteLine("Location weather data was null " + location.Name); }

            //Add Predicted Set of WeatherInfo
            var locationEntity = await _weatherRepository.GetLocationFromId(location.Id);
            if (locationEntity != null)
            {
                var data = await _weatherService.CallWeatherApi(locationEntity, DateTime.Now, DateTime.Now.AddDays(5));
                _homeViewModel.WeatherApiPrediction = data.Select(x => new WeatherModel(x.Temperature, x.Time)).ToList();

                var data2 = await _weatherService.CallMeteoApi(locationEntity, DateTime.Now, DateTime.Now.AddDays(5));
                _homeViewModel.MeteoPrediction = data2.Select(x => new WeatherModel(x.Temperature, x.Time)).ToList();

                Console.WriteLine("weatherapi: " + data.Count);
                Console.WriteLine("meteoapi: " + data2.Count);
            }
            else { Console.WriteLine("Could not load Forecast"); }


                return View("Index", _homeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddLocation(Location model)
        {
            await _weatherRepository.AddLocationAsync(model);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {

            var locations = await _weatherRepository.GetAllLocationsAsync();

            _homeViewModel.Locations.AddRange(
                locations.Select(x => new LocationModel(x.Name, x.Latitude, x.Longitude, x.Id))
            );

            return View(_homeViewModel);
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
