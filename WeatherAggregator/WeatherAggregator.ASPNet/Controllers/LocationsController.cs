using Microsoft.AspNetCore.Mvc;
using WeatherAggregator.ASPNet.Models;
using WeatherAggregator.Library.Database;
using WeatherAggregator.Library.Entities;
using WeatherAggregator.Library.Interfaces;

namespace WeatherAggregator.ASPNet.Controllers
{
    public class LocationsController : Controller
    {
        private readonly IWeatherRepository _weatherRepository;

        public LocationsController(IWeatherRepository repository)
        {
            _weatherRepository = repository;
        }

        // GET: Locations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Locations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Latitude,Longitude")] LocationModel locationModel)
        {
            if (locationModel == null) { return RedirectToAction(nameof(Index), "Home"); }
            var location = new Location(locationModel.Latitude, locationModel.Longitude, locationModel.Name);

            if (ModelState.IsValid)
            {
                await _weatherRepository.AddLocationAsync(location);
                return RedirectToAction(nameof(Index), "Home");
            }
            return View(locationModel);
        }
    }
}
