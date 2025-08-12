using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Binance.Net.Enums;
using BSC_0909.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BSC_0909.Infrastructure.Background
{
    public class BinanceHostedService : IHostedService
    {
        private readonly ILogger<BinanceHostedService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        public BinanceHostedService(ILogger<BinanceHostedService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var binanceService = scope.ServiceProvider.GetRequiredService<BinanceService>();

            _logger.LogInformation("Starting Binance background service...");
            await binanceService.StartKlineStreamAsync(false, "BTCUSDT", KlineInterval.FiveMinutes, cancellationToken);
            _logger.LogInformation("Binance background service started.");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var binanceService = scope.ServiceProvider.GetRequiredService<BinanceService>();

            _logger.LogInformation("Stopping Binance background service...");
            await binanceService.CloseSocketAsync();
            _logger.LogInformation("Binance background service stopped.");
        }
    }
}