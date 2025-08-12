using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BSC_0909.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BSC_0909.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public AppDbContext() { }
        public DbSet<CryptoCurrencyEntity> cryptoCurrencyEntities { get; set; }
        public DbSet<CryptoCurrencyH1Entity> cryptoCurrencyH1Entities { get; set; }
    }
}