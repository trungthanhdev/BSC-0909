using System.Threading.Tasks;
using BSC_0909.Domain.Interfaces;
using BSC_0909.Domain.Services.Bot;
using BSC_0909.Infrastructure.Background;
using BSC_0909.Infrastructure.Persistence;
using BSC_0909.Infrastructure.Persistence.Repository;
using BSC_0909.Infrastructure.Services;
using CTCore.DynamicQuery.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BSC_0909.Infrastructure;

public static class Infrastructure
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        DotNetEnv.Env.Load();
        var opts = new DbContextOptionsBuilder();
        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

        if (string.IsNullOrEmpty(connectionString)) throw new InvalidOperationException("CONNECTION_STRING environment variable is not set.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddScoped(typeof(IRepositoryDefinition<>), typeof(RepositoryDefinition<>));
        services.AddScoped<IUnitOfWork, UnitOfWork<AppDbContext>>();
        services.AddScoped<BinanceService>();
        services.AddHostedService<BinanceHostedService>();
        services.AddHostedService<BinanceHostedServiceH1>();
    }
}

