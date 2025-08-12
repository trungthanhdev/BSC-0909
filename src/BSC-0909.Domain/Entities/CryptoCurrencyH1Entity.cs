using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BSC_0909.Contract.Dtos.Request;
using BSC_0909.Domain.Interfaces;

namespace BSC_0909.Domain.Entities
{
    public class CryptoCurrencyH1Entity : IEntityClass
    {
        [Key]
        public string ccid { get; set; } = null!;
        public string currencyName { get; set; } = null!;
        public decimal OpenPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public DateTimeOffset timeStamp { get; set; }
        private CryptoCurrencyH1Entity() { }
        private CryptoCurrencyH1Entity(ReqSaveCurrentCurrencyInforDto dto)
        {
            this.ccid = Guid.NewGuid().ToString();
            this.currencyName = dto.currencyName;
            this.OpenPrice = dto.OpenPrice;
            this.HighPrice = dto.HighPrice;
            this.LowPrice = dto.LowPrice;
            this.ClosePrice = dto.ClosePrice;
            this.timeStamp = dto.timeStamp;
            checkValid();
        }
        void checkValid()
        {
            if (this.currencyName is null
                 || this.OpenPrice == 0
                 || this.HighPrice == 0
                 || this.LowPrice == 0
                 || this.ClosePrice == 0)
            {
                throw new InvalidOperationException("Can not parse data from Binance!");
            }
        }
        public static CryptoCurrencyH1Entity Create(ReqSaveCurrentCurrencyInforDto dto)
        {
            return new CryptoCurrencyH1Entity(dto);
        }
    }
}