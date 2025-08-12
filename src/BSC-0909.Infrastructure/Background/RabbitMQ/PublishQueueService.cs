using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BSC_0909.Contract.Dtos.Request;
using BSC_0909.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using RabbitMQ.Client;

namespace BSC_0909.Infrastructure.Services
{
    public class PublishQueueService : IRabbitMQPublisher
    {
        private readonly IConnection _conn;
        private readonly ILogger<PublishQueueService> _logger;
        // private readonly IModel _model;
        public PublishQueueService(IConnection conn, ILogger<PublishQueueService> logger)
        {
            _conn = conn;
            _logger = logger;
        }
        async Task IRabbitMQPublisher.PublishAsync(ReqSaveCurrentCurrencyInforDto dto, bool is_H1)
        {
            //exchanges(1) -> routing(1.1) -> queue(2)
            //(1)
            using var channel = await _conn.CreateChannelAsync();
            await channel.ExchangeDeclareAsync("WaitingExchange", ExchangeType.Direct, durable: true, autoDelete: false);

            //(2)
            await channel.QueueDeclareAsync("WaitingQueueExchange", durable: true, exclusive: false, autoDelete: false, arguments: null);

            //(1.1) binding queue to exchange 
            //(exchange, queue, routingkey) waiting.key -> giong 100%, waiting.*: giong tu waiting la binding duoc
            await channel.QueueBindAsync("WaitingQueueExchange", "WaitingExchange", "waiting.key");
            var body = JsonSerializer.SerializeToUtf8Bytes(dto);
            if (is_H1 == true)
            {
                var res = new ReqSaveCurrentCurrencyInforDto
                {
                    is_H1 = true,
                    currencyName = dto.currencyName,
                    OpenPrice = dto.OpenPrice,
                    HighPrice = dto.HighPrice,
                    LowPrice = dto.LowPrice,
                    ClosePrice = dto.ClosePrice,
                    timeStamp = dto.timeStamp
                };
                body = JsonSerializer.SerializeToUtf8Bytes(res);
            }
            //message: de danh dau tung message tranh bi trung lap
            var props = new BasicProperties { MessageId = Guid.NewGuid().ToString("N"), DeliveryMode = DeliveryModes.Persistent, ContentType = "application/json" };

            await channel.BasicPublishAsync(exchange: "WaitingExchange", routingKey: "waiting.key", mandatory: false, basicProperties: props, body: body);
            _logger.LogInformation("Add to queue successfully!");
        }
    }
}