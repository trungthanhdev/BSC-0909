using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSC_0909.Contract.Dtos.Request
{
    public class ReqSaveCurrentCurrencyInforDto
    {
        public string currencyName { get; set; } = null!;
        public decimal OpenPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public DateTimeOffset timeStamp { get; set; }

    }
}