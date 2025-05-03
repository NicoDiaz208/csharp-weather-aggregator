namespace WeatherAggregator.ASPNet.Models
{
    public class WeatherModel
    {
        public WeatherModel() { }

        public WeatherModel(double temperature, DateTime dateTime)
        {
            Temperature = temperature;
            DateTime = dateTime;
        }

        public double Temperature { get; set; }
        public DateTime DateTime { get; set; }
    }
}
