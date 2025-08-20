using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BSC_0909.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BSC_0909.Infrastructure.Indicators
{
    public class RSI : IRSI
    {
        private readonly ILogger<RSI> _logger;
        public RSI(ILogger<RSI> logger) => _logger = logger;
        public IReadOnlyCollection<decimal?> CaculateRSI(List<decimal> closes, bool is_H1, int length)
        {
            if (closes == null) throw new ArgumentNullException(nameof(closes));
            if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length), "length phải > 0");

            int n = closes.Count;
            var rsi = new decimal?[n]; // mặc định null
            if (n < length + 1)
            {
                _logger.LogInformation("RSI{Length} ({Tf}) không đủ nến: cần {Need}, hiện có {Have}.",
                    length, is_H1 ? "H1" : "TF?", length + 1, n);
                return new ReadOnlyCollection<decimal?>(rsi);
            }
            decimal sumGain = 0m, sumLoss = 0m;
            for (int i = 1; i <= length; i++)
            {
                decimal diff = closes[i] - closes[i - 1];
                sumGain += diff > 0 ? diff : 0m;
                sumLoss += diff < 0 ? -diff : 0m;
            }

            decimal avgGain = sumGain / length;
            decimal avgLoss = sumLoss / length;

            // RSI đầu tiên hợp lệ tại index = length
            rsi[length] = AvgToRsi(avgGain, avgLoss);

            //smooth cho cac diem sau
            for (int i = length + 1; i < n; i++)
            {
                decimal diff = closes[i] - closes[i - 1];
                decimal gain = diff > 0 ? diff : 0m;
                decimal loss = diff < 0 ? -diff : 0m;

                avgGain = (avgGain * (length - 1) + gain) / length;
                avgLoss = (avgLoss * (length - 1) + loss) / length;

                rsi[i] = AvgToRsi(avgGain, avgLoss);
            }

            return new ReadOnlyCollection<decimal?>(rsi);
        }

        private static decimal AvgToRsi(decimal avgGain, decimal avgLoss)
        {
            if (avgLoss == 0m) return 100m;
            if (avgGain == 0m) return 0m;
            decimal rs = avgGain / avgLoss;
            return 100m - (100m / (1m + rs));
        }
    }
}
