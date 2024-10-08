using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace financeTracker
{
    internal interface IAccount
    {
        string Name { get; set; }
        string Currency { get; set; }
        decimal Balance { get; set; }
        void Deposite(decimal val);
        void Withdraw(decimal val);
        string ToString();
    }
}
