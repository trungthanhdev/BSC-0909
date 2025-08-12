using System.Threading.Tasks;
using BSC_0909.Domain.Interfaces;
using BSC_0909.Domain.Services.Bot;
using BSC_0909.Infrastructure.Background;
using BSC_0909.Infrastructure.Background.RabbitMQ;
using BSC_0909.Infrastructure.Persistence;
using BSC_0909.Infrastructure.Persistence.Repository;
using BSC_0909.Infrastructure.Services;
using CTCore.DynamicQuery.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace BSC_0909.Infrastructure;

public static class Infrastructure
{
    public static async Task AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
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

        //RabitMQ
        var hostName = Environment.GetEnvironmentVariable("HOST_NAME_RBMQ") ?? throw new InvalidOperationException("HOST_NAME_RBMQ not found!");
        var port = Environment.GetEnvironmentVariable("PORT_RBMQ") ?? throw new InvalidOperationException("PORT_RBMQ not found!");
        if (!int.TryParse(port, out var portNumber))
        {
            throw new InvalidOperationException("Can not parse port RabbitMQ!");
        }
        var userName = Environment.GetEnvironmentVariable("USER_NAME_RBMQ") ?? throw new InvalidOperationException("USER_NAME_RBMQ not found!");
        var passWord = Environment.GetEnvironmentVariable("PASS_RBMQ") ?? throw new InvalidOperationException("PASS_RBMQ not found!");
        var factory = new ConnectionFactory()
        {
            HostName = hostName,
            Port = portNumber,
            UserName = userName,
            Password = passWord,
            VirtualHost = "/"
        };
        var connectionRabbitMQ = await factory.CreateConnectionAsync();
        services.AddSingleton<IConnection>(connectionRabbitMQ);
        services.AddSingleton<IRabbitMQPublisher, PublishQueueService>();
        services.AddHostedService<CustomerQueueService>();
    }
}

