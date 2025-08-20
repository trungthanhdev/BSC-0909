using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSC_0909.Domain.Interfaces
{
    public interface IRSI
    {
        IReadOnlyCollection<decimal?> CaculateRSI(List<decimal> closes, bool is_H1, int length);
    }
}