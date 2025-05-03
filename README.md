# csharp-weather-aggregator
A small csharp application which displays weather information using postgresql and EF-Core


 - Docker Postgres
    - if you dont have a running postgres database use this 
    ```bash docker run --name weatheraggregator -e POSTGRES_PASSWORD=xxxx -e POSTGRES_DB=weather_db -p 5432:5432 -d postgres ``` (The used password in the code is just a example for testing purpose, please change if you use this code)
 - EF-Integration
    - Download tool if not already
    ```bash dotnet tool install --global dotnet-ef ```
    - Delete Migration in WeatherAggregator.Library and run following:
    ```bash dotnet ef migrations add InitSchema --project WeatherAggregator.Library --startup-project WeatherAggregator.ASPNet dotnet ef database update ```


Todos for Project WeatherAggregator:
  - [ ]  Import more features to entity weatherinfo like air_pressure, humidity etc.
  - [ ]  Create Graph view of data with option to see Temperature, air_pressure etc over time.
  - [ ]  Add button to activate background WeatherPollingService to automatically import data every time.
  - [ ]  improve async and parallel shit
  - [ ]  Add create page to add new locations
