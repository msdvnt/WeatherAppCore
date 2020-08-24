using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeatherAppCore.Models;

namespace WeatherAppCore.Data
{
    public class WeatherAppCoreContext : DbContext
    {
        public WeatherAppCoreContext (DbContextOptions<WeatherAppCoreContext> options)
            : base(options)
        {
        }

        public DbSet<WeatherAppCore.Models.WeatherData> WeatherData { get; set; }
    }
}
