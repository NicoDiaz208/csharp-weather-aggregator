using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAggregator.Library.Entities;
using WeatherAggregator.Library.Interfaces.Entities;

namespace WeatherAggregator.Library.Interfaces
{
    public interface IWeatherClientApiService
    {
        Task<WeatherInfo> GetWeatherAsync(ILocation location);
    }
}
