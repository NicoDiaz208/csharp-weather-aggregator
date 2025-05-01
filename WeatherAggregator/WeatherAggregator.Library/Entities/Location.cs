using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAggregator.Library.Interfaces.Entities;

namespace WeatherAggregator.Library.Entities
{
    public class Location : ILocation
    {
        public Location(double latitude, double longitude, string name)
        {
            Latitude = latitude;
            Longitude = longitude;
            Name = name;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; set; }
    }
}
