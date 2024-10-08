using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace financeTracker
{
    internal class User : IUser
    {
        public string UserName { get; set; }
        public string Plan { get; set; }
        public List<string> Accounts { get; set; }

        public User()
        {
            UserName = string.Empty;
            Plan = string.Empty;
            Accounts = new List<string>();
        }

        public User(string name)
        {
            UserName=name;
            Plan = "basic";
            Accounts = new List<string>();
        }
        public User(string name, string plan)
        {
            UserName = name;
            Plan = plan;
            Accounts = new List<string>();
        }
        public User(string name, string plan, List<string> accounts)
        {
            UserName = name;
            Plan = plan;
            Accounts = accounts;
        }

        // The method calculates the Total balance on the account in USD
        public decimal TotalBalance(IAccountList accountList)
        {
            decimal total = 0;
            foreach (var account in Accounts)
            {
                Account acc = accountList.GetAccount(account);
                total += BalanceUSD(acc.Balance,acc.Currency);
            }
            return total;
        }

        // The method calculates the balance on the account in USD
        public decimal BalanceUSD(decimal balance, string currency)
        {
            decimal value = 0;
            decimal currentRate = 0;
            decimal usdRate = 0;
            try
            {
                IRates rates = new Rates("rites.json");
                foreach (var rate in rates.RatesList)
                {
                    if (rate.CurrencyName == currency)
                    {
                        currentRate = rate.CurrencyRate;
                    }
                    if (rate.CurrencyName == "USD"){
                        usdRate = rate.CurrencyRate;
                    }
                }
                value = decimal.Round(balance * currentRate / usdRate,2);
            }
            catch (Exception)
            {
                throw new Exception("Rates not found");
            }
            
            return value;
        }
        public int CountAccount()
        {
            return Accounts.Count;
        }

        public void AddAccount(string name)
        {
            Accounts.Add(name);
        }
    }
}
