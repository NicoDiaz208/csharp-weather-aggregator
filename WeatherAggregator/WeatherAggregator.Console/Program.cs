using WeatherAggregator.Library.Service;

var service = new WeatherService();
var call = await service.GetWeatherAsync(52.52, 13.41);

if (call != null)
{
    Console.WriteLine($"The Weather in Linz is: {call.Temperature}");
}
else
{
    Console.WriteLine("Is null du huso");
}


