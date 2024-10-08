using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace financeTracker
{
    internal interface IRate
    {
        string CurrencyName { get; }
        decimal CurrencyRate { get; }
    }
}
