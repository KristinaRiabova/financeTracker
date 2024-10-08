using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace financeTracker
{
    internal interface IActions
    {
        void Options(string accountName);
        void Info(Account account);
        void Convert(ref Account account, string currency, IRates rates);
        void History(List<Transaction> transaction, string accountName);


    }
}
