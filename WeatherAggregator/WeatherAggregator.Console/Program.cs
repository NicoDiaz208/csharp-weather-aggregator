using WeatherAggregator.Library.Database;
using WeatherAggregator.Library.Entities;
using WeatherAggregator.Library.Interfaces;
using WeatherAggregator.Library.Service;
using Microsoft.EntityFrameworkCore;
using WeatherAggregator.Library;
using WeatherAggregator.Library.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

var service = new WeatherService(new HttpClient());
var location = new Location(52.52, 13.41, "idk");
var call = await service.GetWeatherAsync(location);

if (call != null)
{
    Console.WriteLine($"The Weather in Linz is: {call.Temperature}");
}
else
{
    Console.WriteLine("Is null du huso");
}


var builder = Host.CreateApplicationBuilder(args);

// Configure PostgreSQL EF Core
builder.Services.AddDbContext<WeatherDbContext>(options =>
    options.UseNpgsql("Host=localhost;Database=weather_db;Username=postgres;Password=passme01"));

// Register service
builder.Services.AddScoped<WeatherRepository>();

var app = builder.Build();

// Auto-apply migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();
    db.Database.Migrate();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();

    var data = db.WeatherInfos
                 .Include(w => w.Location)
                 .ToList();

    foreach (var w in data)
    {
        Console.WriteLine($"{w.Time}: {w.Temperature}°C in {w.Location.Name} (Lat: {w.Location.Latitude}, Lon: {w.Location.Longitude})");
    }
}

