using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace financeTracker
{
    public class Account: IAccount
    {
        public decimal Balance { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }

        public Account()
        {
            Name = "";
            Currency = "UAH";
            Balance = 0;
        }
        public Account(string name, string currency) 
        {
            Name = name;
            Currency = currency;
            Balance = 0;
        }
        public Account(string name, string currency, decimal balance)
        {
            Name = name;
            Currency = currency;
            Balance = balance;
        }
        public void Deposite(decimal val)
        {
            Balance += val;
        }
        public void Withdraw(decimal val)
        {
            // When withdrawing from an account, the availability of funds in the account is checked,
            // otherwise we throw an error
            if (Balance - val< 0)
            {
                throw new ArgumentException("The amount in the account is not sufficient to carry out the transaction.");
            }
            Balance -= val;
        }

        public override string ToString()
        {
            return $"AccountName: {Name},\nAccountCurrency: {Currency},\nAccountBalance: {Balance}";
        }
    }
}
