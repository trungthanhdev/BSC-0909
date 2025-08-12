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
    public class BinanceHostedServiceH1 : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BinanceHostedServiceH1> _logger;
        public BinanceHostedServiceH1(IServiceScopeFactory scopeFactory, ILogger<BinanceHostedServiceH1> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var binanceServiceH1 = scope.ServiceProvider.GetRequiredService<BinanceService>();
            _logger.LogInformation("Starting Binance background H1 service...");
            await binanceServiceH1.StartKlineStreamAsync(true, "BTCUSDT", KlineInterval.OneHour, cancellationToken);
            _logger.LogInformation("Binance background service H1 started.");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var binanceServiceM15 = scope.ServiceProvider.GetRequiredService<BinanceService>();
            _logger.LogInformation("Stopping Binance background service...");
            await binanceServiceM15.CloseSocketAsync();
            _logger.LogInformation("Binance background service stopped.");
        }
    }
}