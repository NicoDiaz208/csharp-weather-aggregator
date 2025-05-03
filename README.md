# csharp-weather-aggregator
A small csharp application which displays weather information using postgresql and EF-Core

 - EF-Integration
  dotnet tool install --global dotnet-ef
   dotnet ef migrations add InitSchema --project WeatherAggregator.Library --startup-project WeatherAggregator.Console

