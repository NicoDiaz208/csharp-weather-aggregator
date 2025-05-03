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
        Task AddAsync(WeatherInfo weather);
    }
}
