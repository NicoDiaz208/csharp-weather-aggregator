# csharp-weather-aggregator
A small csharp application which displays weather information using postgresql and EF-Core

 - EF-Integration
    - Download tool if not already
    ```bash dotnet tool install --global dotnet-ef ```
    - Delete Migration in WeatherAggregator.Library and run following:
    ```bash dotnet ef migrations add InitSchema --project WeatherAggregator.Library --startup-project WeatherAggregator.ASPNet dotnet ef database update ``` 
