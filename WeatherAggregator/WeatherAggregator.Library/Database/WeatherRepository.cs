using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAggregator.Library.Entities;
using WeatherAggregator.Library.Interfaces;

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

        public async Task AddAsync(WeatherInfo weather)
        {
            await _context.WeatherInfos.AddAsync(weather);
            await _context.SaveChangesAsync();
        }
    }
}
