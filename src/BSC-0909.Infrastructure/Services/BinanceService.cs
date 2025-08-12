using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Objects.Models.Spot;
using BSC_0909.Contract.Dtos.Request;
using BSC_0909.Domain.Entities;
using BSC_0909.Domain.Interfaces;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects.Sockets;
using CTCore.DynamicQuery.Core.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BSC_0909.Infrastructure.Services
{
    public class BinanceService
    {
        private readonly string BinanceAPIKey = Environment.GetEnvironmentVariable("BINANCE_API_KEY") ?? throw new InvalidOperationException("BinanceAPIKey not found!");
        private readonly string BinanceAPISecret = Environment.GetEnvironmentVariable("BINANCE_SECRET_KEY") ?? throw new InvalidOperationException("BinanceAPISecret not found!");
        private readonly IRepositoryDefinition<CryptoCurrencyEntity>? _cceRepo;
        private readonly BinanceSocketClient? _binanceClient;
        private readonly ILogger<BinanceService> _logger;
        private UpdateSubscription? _subscription;
        private readonly IServiceScopeFactory _scopeFactory;
        public BinanceService(IRepositoryDefinition<CryptoCurrencyEntity> cceRepo, ILogger<BinanceService> logger, IServiceScopeFactory scopeFactory)
        {
            _binanceClient = new BinanceSocketClient(opts =>
            {
                opts.ReconnectInterval = TimeSpan.FromSeconds(1);
            });
            _cceRepo = cceRepo;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }
        public async Task StartKlineStreamAsync(bool is_H1, string symbol = "BTCUSDT", KlineInterval tf = KlineInterval.FiveMinutes, CancellationToken ct = default)
        {
            var res = await _binanceClient!.UsdFuturesApi.ExchangeData.SubscribeToKlineUpdatesAsync(symbol, tf, async msg =>
            {
                var k = msg.Data;
                if (k.Data.Final)
                {
                    Console.WriteLine($"[KLINE {symbol} {tf}] O:{k.Data.OpenPrice} H:{k.Data.HighPrice} L:{k.Data.LowPrice} C:{k.Data.ClosePrice} Close={k.Data.CloseTime:HH:mm:ss}");
                    var dto = k.Data;
                    ReqSaveCurrentCurrencyInforDto obj = new ReqSaveCurrentCurrencyInforDto
                    {
                        currencyName = symbol,
                        OpenPrice = dto.OpenPrice,
                        HighPrice = dto.HighPrice,
                        LowPrice = dto.LowPrice,
                        ClosePrice = dto.ClosePrice,
                        timeStamp = dto.CloseTime
                    };

                    using var scope = _scopeFactory.CreateScope();
                    var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    if (is_H1 == false)
                    {
                        var newData = CryptoCurrencyEntity.Create(obj);
                        var repo = scope.ServiceProvider.GetRequiredService<IRepositoryDefinition<CryptoCurrencyEntity>>();
                        repo.Add(newData);
                        if (await uow.SaveChangeAsync(ct) > 0)
                        {
                            _logger.LogInformation("Save successfully!");
                        }
                        else
                        {
                            _logger.LogError("Can not save!");
                        }
                    }
                    else
                    {
                        try { await Task.Delay(TimeSpan.FromSeconds(3), ct); }
                        catch (TaskCanceledException) { return; }
                        var newData = CryptoCurrencyH1Entity.Create(obj);
                        var repo = scope.ServiceProvider.GetRequiredService<IRepositoryDefinition<CryptoCurrencyH1Entity>>();
                        repo.Add(newData);
                        if (await uow.SaveChangeAsync(ct) > 0)
                        {
                            _logger.LogInformation("Save successfully!");
                        }
                        else
                        {
                            _logger.LogError("Can not save!");
                        }
                    }
                    // TODO: tính tín hiệu của bạn tại đây (EMA/RSI/…)
                    // TODO: gửi Telegram nếu cần
                }
            });

            if (!res.Success) throw new Exception($"Kline subscribe error: {res.Error}");
            _subscription = res.Data;
            _logger.LogInformation("Kline stream started for {symbol} {tf}", symbol, tf);
        }

        public async Task CloseSocketAsync()
        {
            if (_subscription != null)
            {
                await _subscription.CloseAsync();
                _logger.LogInformation("Binance socket closed.");
            }
            _binanceClient?.Dispose();
        }
    }
}