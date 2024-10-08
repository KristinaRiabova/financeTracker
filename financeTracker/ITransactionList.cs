using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace financeTracker
{
    internal interface ITransactionList
    {
        string FileName { get; }
        List<Transaction> Transactions { get; }
        List<Transaction> GetTransactions(string accountName);
        void AddRecord(Transaction transaction);

        void Export(string exportFile);
        void Info();

    }
}
