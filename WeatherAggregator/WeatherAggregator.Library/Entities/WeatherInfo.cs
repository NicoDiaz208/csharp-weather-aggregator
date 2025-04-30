using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAggregator.Library.Interfaces;

namespace WeatherAggregator.Library.Entities
{
    public class WeatherInfo: IWeatherInfo
    {
        public double Temperature { get; set; }
        public string Time { get; set; }
    }
}
