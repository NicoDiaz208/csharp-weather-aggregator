using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAggregator.Library.Entities;

namespace WeatherAggregator.Library.Database
{
    public class WeatherDbContext: DbContext
    {
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options): base(options)
        {}

        public DbSet<WeatherInfo> WeatherInfos { get; set; }
        public DbSet<Location> Locations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>()
                        .HasMany(l => l.WeatherEntries)
                        .WithOne(w => w.Location)
                        .HasForeignKey(w => w.LocationId);
        }
    }
}
