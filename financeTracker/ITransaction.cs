using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace financeTracker
{

    internal interface ITransaction
    {
        string Action { get; }
        decimal Amount { get; }
        string AccountName { get; }
        string Comment { get; }
        string Date { get; }

        string ToString();
    }
}
