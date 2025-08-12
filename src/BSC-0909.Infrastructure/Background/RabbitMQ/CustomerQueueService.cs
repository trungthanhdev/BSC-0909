using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BSC_0909.Contract.Dtos.Request;
using BSC_0909.Domain.Entities;
using BSC_0909.Domain.Interfaces;
using CTCore.DynamicQuery.Core.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BSC_0909.Infrastructure.Background.RabbitMQ
{
    public class CustomerQueueService : IHostedService
    {
        private readonly IConnection _conn;
        private readonly ILogger<CustomerQueueService> _logger;
        private IChannel? _ch;
        private readonly IServiceScopeFactory _scopeFactory;
        private const string queue = "WaitingQueueExchange";
        public CustomerQueueService(IConnection conn, ILogger<CustomerQueueService> logger, IServiceScopeFactory scopeFactory)
        {
            _conn = conn;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _ch = await _conn.CreateChannelAsync();
            //prefetchCount: moi lan chi gui 1 message xuong de xu ly
            await _ch.BasicQosAsync(0u, prefetchCount: 1, global: false, cancellationToken: cancellationToken);

            //tao consumer de nhan message (xu ly data)
            var consumer = new AsyncEventingBasicConsumer(_ch);
            consumer.ReceivedAsync += async (model, ea) =>
                {
                    _logger.LogInformation($"CustomerQueueService consuming {queue}");
                    var deliveryTag = ea.DeliveryTag;
                    try
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        byte[] body = ea.Body.ToArray();
                        var message = JsonSerializer.Deserialize<ReqSaveCurrentCurrencyInforDto>(body);
                        if (message is null) throw new InvalidOperationException("Can not parse data in CustomerQueueService");
                        if (message?.is_H1 == true)
                        {
                            var h1Repo = scope.ServiceProvider.GetRequiredService<IRepositoryDefinition<CryptoCurrencyH1Entity>>();
                            var newData = CryptoCurrencyH1Entity.Create(message);
                            h1Repo.Add(newData);
                        }
                        else
                        {
                            var m5Repo = scope.ServiceProvider.GetRequiredService<IRepositoryDefinition<CryptoCurrencyEntity>>();
                            var mapObj = new ReqSaveCurrentCurrencyInforDto
                            {
                                currencyName = message!.currencyName,
                                OpenPrice = message.OpenPrice,
                                HighPrice = message.HighPrice,
                                LowPrice = message.LowPrice,
                                ClosePrice = message.ClosePrice,
                                timeStamp = message.timeStamp
                            };
                            var newData = CryptoCurrencyEntity.Create(mapObj);
                            m5Repo.Add(newData);
                        }
                        if (await uow.SaveChangeAsync(cancellationToken) > 0)
                        {
                            await _ch.BasicAckAsync(deliveryTag, false);
                            _logger.LogInformation($"Save to db successfully! {queue}");
                        }
                    }
                    catch (System.Exception ex)
                    {
                        _logger.LogError($"Message: {ex}");
                        await _ch.BasicNackAsync(deliveryTag, false, false);
                        throw;
                    }
                };
            //bat dau xu ly queue, dang ki queue, khi co message moi -> goi ham ReceivedAsync
            await _ch.BasicConsumeAsync(queue, false, consumer: consumer);
        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _ch!.CloseAsync(cancellationToken); _ch = null;
        }
    }
}