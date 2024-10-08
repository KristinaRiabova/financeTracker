using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace financeTracker
{
    internal interface IRates
    {
        List<Rate> RatesList { get; }
        Rate GetRate(string currency);
    }
}
