using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BSC_0909.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BSC_0909.Infrastructure.Extension
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            DotNetEnv.Env.Load(Path.Combine("..", "BSC-0909.Controller", ".env"));
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            optionsBuilder.UseNpgsql(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}