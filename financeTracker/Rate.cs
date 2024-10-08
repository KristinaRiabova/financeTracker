using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace financeTracker
{
    internal class Rate : IRate
    {
        public string CurrencyName { get; set; } = string.Empty;
        public decimal CurrencyRate { get; set; } = 0;
        public Rate()
        {
            CurrencyName = string.Empty;
            CurrencyRate = 0;
        }
        public Rate(string name, decimal val) 
        { 
            CurrencyName = name;
            CurrencyRate = val;
        }
    }
}
