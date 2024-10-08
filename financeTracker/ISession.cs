using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace financeTracker
{
    internal interface ISession
    {
        User? ActiveUser { get; set; }
        User? Registered(User user);
        void Register(User user);
        void ChangePlan(string plan, IAccountList accountList);
        void AddLog(string json);
        void AddAccount(string name);
    }
}
