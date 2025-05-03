using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAggregator.Library.Entities;

namespace WeatherAggregator.Library.Interfaces
{
    public interface IWeatherRepository
    {
        Task<List<WeatherInfo>> GetAllAsync();
        Task AddWeatherAsync(WeatherInfo weather);
        Task AddLocationAsync(Location location);
        Task<List<WeatherInfo>> GetWeatherInfoFromLocation(int locationId);
        Task<List<Location>> GetAllLocations();
        void AddAllWeatherAsync(List<WeatherInfo> data);

    }
}
