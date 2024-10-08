using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace financeTracker
{
    internal interface IAccountList
    {
        List<Account> Accounts { get; }
        string Source { get; }
        void AddAccount(string name, string currency);
        void SaveAccount(Account account);
        Account GetAccount(string name);
        void Deposite(string name, decimal value);
        void Withdraw(string name, decimal value);
        void Info();

    }
}
