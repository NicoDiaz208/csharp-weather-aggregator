using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAggregator.Library.Interfaces.Entities
{
    public interface ILocation
    {
        double Latitude { get; set; }
        double Longitude { get; set; }
        string Name { get; set; }
    }
}
