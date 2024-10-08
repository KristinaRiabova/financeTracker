using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace financeTracker
{
    internal interface IUser
    {
        string UserName { get; set; }
        string Plan { get; set; }
        List<string> Accounts { get; set; }

        decimal TotalBalance(IAccountList accountList);

        int CountAccount();
        void AddAccount(string name);
    }
}
