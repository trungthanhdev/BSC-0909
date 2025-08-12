using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BSC_0909.Contract.Dtos.Request;

namespace BSC_0909.Domain.Interfaces
{
    public interface IRabbitMQPublisher
    {
        Task PublishAsync(ReqSaveCurrentCurrencyInforDto dto, bool is_H1);
    }
}