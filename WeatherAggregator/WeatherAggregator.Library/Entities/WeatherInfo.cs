using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAggregator.Library.Interfaces.Entities;

namespace WeatherAggregator.Library.Entities
{
    public class WeatherInfo: IWeatherInfo
    {
        public int Id { get; set; }
        public double Temperature { get; set; }
        public string Time { get; set; }
        public int LocationId { get; set; }

        [ForeignKey(nameof(LocationId))]
        public Location Location { get; set; } = null!;
    }
}
