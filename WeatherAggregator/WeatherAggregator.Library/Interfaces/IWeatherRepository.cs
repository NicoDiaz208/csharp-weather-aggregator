using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAggregator.Library.Entities;
using WeatherAggregator.Library.Interfaces.Entities;

namespace WeatherAggregator.Library.Interfaces
{
    public interface IWeatherRepository
    {
        Task<List<WeatherInfo>> GetAllAsync();
        Task AddWeatherAsync(WeatherInfo weather);
        Task AddLocationAsync(Location location);
        Task<List<WeatherInfo>> GetWeatherInfoFromLocation(int locationId);
        Task<List<Location>> GetAllLocationsAsync();
        Task AddAllWeatherAsync(List<WeatherInfo> data);
        Task<bool> DeleteLocation(int locationId);

        Task<Location?> GetLocationFromId(int id);

    }
}
