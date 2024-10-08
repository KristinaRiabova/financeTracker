using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static System.Collections.Specialized.BitVector32;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace financeTracker
{
    internal class Transaction: ITransaction
    {
        public string Action { get; set; }
        public decimal Amount { get; set; }
        public string AccountName { get; set; }
        public string Comment { get; set; }
        public string Date { get; set; }

        public Transaction()
        {
            Action = string.Empty;
            Amount = 0;
            AccountName = string.Empty;
            Comment = string.Empty;
            Date = DateTime.Now.ToString();
        }
        public Transaction(string action, decimal amount, string accountName, string comment, string date) 
        {
            Action = action;
            Amount = amount;
            AccountName = accountName;
            Comment = comment;
            Date = date;
        }
        public override string ToString()
        {
            return $"{Date,-16} {Action,-10} {AccountName,-12} {Amount,-10} {Comment}";
        }

    }
}
