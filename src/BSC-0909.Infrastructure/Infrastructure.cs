using BSC_0909.Domain.Services.Bot;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BSC_0909.Infrastructure;

public static class Infrastructure
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // DotNetEnv.Env.Load();
        // var opts = new DbContextOptionsBuilder();
        // var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

        // if (string.IsNullOrEmpty(connectionString)) throw new InvalidOperationException("CONNECTION_STRING environment variable is not set.");

        // services.AddDbContext<AppDbContext>(options =>
        //     options.UseNpgsql(connectionString));
        services.AddScoped<BotModelService>();
        var botService = new BotModelService();
        botService.SendMessageToTelegram("abc");
    }
}
