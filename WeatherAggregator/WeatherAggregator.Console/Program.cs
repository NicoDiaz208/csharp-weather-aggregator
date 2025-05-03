using WeatherAggregator.Library.Database;
using WeatherAggregator.Library.Entities;
using WeatherAggregator.Library.Interfaces;
using WeatherAggregator.Library.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;


Console.WriteLine("What do you want to execute?");
Console.WriteLine("1: Api call");
Console.WriteLine("2: read Database");
Console.WriteLine("3: import csv data to database");
Console.WriteLine("4: add a new location");

var key = Console.ReadKey();
switch (key.Key){
    case ConsoleKey.D1:
        await callApi();
        break;
    case ConsoleKey.D2:
         await ReadDatabase();
        break;

case ConsoleKey.D3:
        await ImportAndAddToDatabase();
        break;
    case ConsoleKey.D4: 
        await AddLocation();
        break;
}

async Task callApi()
{
    var service = new WeatherApiClientService(new HttpClient());
    var location = new Location(52.52, 13.41, "idk");
    var call = await service.GetWeatherAsync(location);

    if (call != null)
    {
        Console.WriteLine($"The Weather in Linz is: {call.Temperature}");
    }
    else
    {
        Console.WriteLine("Is null");
    }
}

async Task AddLocation()
{
    Console.Write("Location Name: ");
    String? name = Console.ReadLine();
    Console.Write("Location Latitude: ");
    String? latitude = Console.ReadLine();
    Console.Write("Location Longitude: ");
    String? longitude = Console.ReadLine();

    if(name == null || latitude == null || longitude == null)
    {
        Console.WriteLine("Someting went wrong");
        return;
    }

    var builder = Host.CreateApplicationBuilder(args);

    // Configure PostgreSQL EF Core
    builder.Services.AddDbContext<WeatherDbContext>(options =>
        options.UseNpgsql("Host=localhost;Database=weather_db;Username=postgres;Password=passme01")); //just for testing, should be changed in real case

    // Register service
    builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();

    var app = builder.Build();

    // Auto-apply migrations
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();
        db.Database.Migrate();
    }

    using (var scope = app.Services.CreateScope())
    {
        var repo = scope.ServiceProvider.GetRequiredService<IWeatherRepository>();
        var locations = await repo.GetAllLocationsAsync();
        Location location = locations.Where(x => x.Name == name).FirstOrDefault();
        if ( location == null)
        {
            location = new Location(double.Parse(latitude), double.Parse(longitude), name);
            await repo.AddLocationAsync(location);
            Console.WriteLine("successfully added location");
        }

        var service = new WeatherApiClientService(new HttpClient());
        var weatherinfo = await service.GetWeatherAsync(location);
        weatherinfo.LocationId = location.Id;
        weatherinfo.Location = location;

        await repo.AddWeatherAsync(weatherinfo);
        Console.WriteLine("successfully added new temp time thing.");
    }

}

async Task ImportAndAddToDatabase()
{
    Console.WriteLine("Filepath: ");
    String? filepath = Console.ReadLine();
    Console.WriteLine("Which Location?");
    String? locationName = Console.ReadLine();

    var builder = Host.CreateApplicationBuilder(args);

    // Configure PostgreSQL EF Core
    builder.Services.AddDbContext<WeatherDbContext>(options =>
        options.UseNpgsql("Host=localhost;Database=weather_db;Username=postgres;Password=passme01"));

    // Register service
    builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();

    var app = builder.Build();

    // Auto-apply migrations
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();
        db.Database.Migrate();
    }

    using (var scope = app.Services.CreateScope())
    {
        var repo = scope.ServiceProvider.GetRequiredService<IWeatherRepository>();

        var locations = await repo.GetAllLocationsAsync();
        var location = locations.Where(x => x.Name == locationName).FirstOrDefault();

        DataCSVImporter importer = new DataCSVImporter(repo);

        if (location == null || filepath == null)
        {
            Console.WriteLine("Error occured.");
            return;
        }
        importer.ImportAndJoin(filepath, location);
    }
}
    

 async Task ReadDatabase()
{
    var builder = Host.CreateApplicationBuilder(args);

    // Configure PostgreSQL EF Core
    builder.Services.AddDbContext<WeatherDbContext>(options =>
        options.UseNpgsql("Host=localhost;Database=weather_db;Username=postgres;Password=passme01"));

    // Register service
    builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();

    var app = builder.Build();

    // Auto-apply migrations
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();
        db.Database.Migrate();
    }

    using (var scope = app.Services.CreateScope())
    {
        var repo = scope.ServiceProvider.GetRequiredService<IWeatherRepository>();

        var all = await repo.GetAllAsync();
        foreach (var entry in all)
        {
            Console.WriteLine($"{entry.LocationId} {entry.Time} {entry.Temperature}");

        }
    }
}