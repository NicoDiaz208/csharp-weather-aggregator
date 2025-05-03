namespace WeatherAggregator.ASPNet.Models
{
    public class LocationModel
    {
        public LocationModel() { }

        public LocationModel(string name, double latitude, double longitude, int id)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
            Id = id;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
