using WeatherAggregator.Library.Entities;

namespace WeatherAggregator.ASPNet.Models
{
    public class HomeViewModel
    {
        public List<LocationModel> Locations { get; set; } = new();
        public List<WeatherModel> WeatherModels { get; set; } = new();
        public List<WeatherModel> MeteoPrediction { get; set; } = new();
        public List<WeatherModel> WeatherApiPrediction{ get; set; } = new(); 
        public LocationModel CurrentLocation { get; set; } = new();
    }
}
