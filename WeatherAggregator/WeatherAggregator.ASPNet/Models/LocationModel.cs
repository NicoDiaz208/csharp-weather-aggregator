namespace WeatherAggregator.ASPNet.Models
{
    public class LocationModel
    {
        public LocationModel() { }

        public LocationModel(string name, double latitude, double longitude)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }

        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
