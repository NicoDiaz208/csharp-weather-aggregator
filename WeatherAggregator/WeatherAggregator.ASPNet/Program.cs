using Microsoft.EntityFrameworkCore;
using WeatherAggregator.Library.Database;
using WeatherAggregator.Library.Interfaces;
using WeatherAggregator.Library.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IWeatherClientApiService, WeatherApiClientService>();

builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();

// Configure PostgreSQL EF Core
builder.Services.AddDbContext<WeatherDbContext>(options =>
    options.UseNpgsql("Host=localhost;Database=weather_db;Username=postgres;Password=passme01"));

var app = builder.Build();


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
