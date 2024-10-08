using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace financeTracker
{
    internal static class Actions
    {
        public static void Options(string accountName)
        {
            Console.WriteLine("Available actions:");
            Console.WriteLine("\tinfo: view details of account");
            Console.WriteLine("\tconvert: convert the account balance to the specified currency (for account in foreign currencies)");
            Console.WriteLine("\thistory: displays a list of transactions associated with the account");

            Console.WriteLine($"\tExapmle:\n\t\taction info {accountName}");
        }

        public static void Info(Account account)
        {
            Console.WriteLine("Account details:");
            Console.WriteLine($"Account name: {account.Name}");
            Console.WriteLine($"Currency: {account.Currency}");
            Console.WriteLine($"Balance: {account.Balance}");
        }
        public static void Convert(ref Account account, string currency, IRates rates)
        {
                if (account.Currency != "UAH" && account.Currency != currency)
                {
                    decimal value = account.Balance;
                    decimal courseCurrencyOld = rates.GetRate(account.Currency).CurrencyRate;
                    decimal courseCurrencyNew = rates.GetRate(currency).CurrencyRate;
                    account.Balance = Decimal.Round(value * courseCurrencyOld / courseCurrencyNew, 2);
                    account.Currency = currency;
                }

        }
        public static void History(List<Transaction> transaction, string accountName)
        {
            foreach (Transaction t in transaction)
            {
                Console.WriteLine(t.ToString());
            }
        }
    }
}
