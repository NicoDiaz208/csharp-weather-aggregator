using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WeatherAggregator.Library.Database;
using WeatherAggregator.Library.Interfaces;
using WeatherAggregator.Library.Service;

var builder = WebApplication.CreateBuilder(args);

// Rebuild the configuration pipeline explicitly
builder.Configuration.AddUserSecrets<Program>().AddEnvironmentVariables();


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IWeatherClientApiService, WeatherApiClientService>();

builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();

// Configure PostgreSQL EF Core

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<WeatherDbContext>(options =>
    options.UseNpgsql(connectionString)); 

var app = builder.Build();

// Force culture to en-US to handle dot decimals correctly
var enUs = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = enUs;
CultureInfo.DefaultThreadCurrentUICulture = enUs;

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
