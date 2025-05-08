using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAggregator.Library.Entities;
using WeatherAggregator.Library.Interfaces;
using WeatherAggregator.Library.Interfaces.Entities;

namespace WeatherAggregator.Library.Database
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly WeatherDbContext _context;

        public WeatherRepository(WeatherDbContext context)
        {
            _context = context;
        }

        public async Task<List<WeatherInfo>> GetAllAsync()
        {
            return await _context.WeatherInfos.ToListAsync();
        }

        public async Task<Location?> GetLocationFromId(int id)
        {
            return await _context.Locations.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task AddWeatherAsync(WeatherInfo weather)
        {
            weather.Time = DateTime.SpecifyKind(weather.Time, DateTimeKind.Utc);
            await _context.WeatherInfos.AddAsync(weather);
            await _context.SaveChangesAsync();
        }

        public async Task AddLocationAsync(Location location)
        {
            await _context.Locations.AddAsync(location);
            await _context.SaveChangesAsync();
        }

        public async Task<List<WeatherInfo>> GetWeatherInfoFromLocation(int locationId)
        {
            return await _context.WeatherInfos.Where(x => x.LocationId == locationId).ToListAsync();
        }

        public async Task<List<Location>> GetAllLocationsAsync()
        {
            return await _context.Locations.ToListAsync();
        }
         
        public async Task AddAllWeatherAsync(List<WeatherInfo> data)
        {


            foreach(var x in data)
            {
                if (x.Location == null) 
                    continue;

                await _context.AddAsync(x);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteLocation(int locationId)
        {
            var todelete = await _context.Locations.Where(x => x.Id == locationId).FirstOrDefaultAsync();
            if (todelete != null)
            {
                _context.Remove(todelete);
                await _context.SaveChangesAsync();
                return true;
            }

            return  false;
        }
    }
}
