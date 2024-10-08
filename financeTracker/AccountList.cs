using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace financeTracker
{
    // Using interfaces for data stored in files is not possible because deserialization is not supported
    // in class interfaces.
    internal class AccountList : IAccountList
    {
        public List<Account> Accounts { get; private set; }
        public string Source { get; private set; }
        public AccountList(string source) 
        {
            Source = source;
            
            if (!File.Exists(Source))
            {
                File.WriteAllText(Source,"[]");
            }

            // Read Account with file Source

            string jsonString = File.ReadAllText(Source);
            try
            {
                Accounts = JsonSerializer.Deserialize<List<Account>>(jsonString) ?? new List<Account>();
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentException("Argument Null Exception");
            }
            catch (JsonException)
            {
                throw new ArgumentException("JSON Exception");
            }
            catch (NotSupportedException)
            {
                throw new ArgumentException("Not supported Exception");
            }
            catch (Exception)
            {
                throw new ArgumentException("unknown error");
            }
        }
        public void AddAccount(string name, string currency)
        {
            try
            {
                // let's use the method GetAccount to determine the existence of an account
                // if the account does not exist, the method throws an exception
                IAccount account = GetAccount(name);
                Console.WriteLine($"account {account.Name} already exists");
            }
            catch (ArgumentException)
            {
                // if the method throws an exception, then the account does not exist, you can create it
                Account account = new Account(name, currency);
                Accounts.Add(account);
                SaveAccount(account);

            }
        }
        public void SaveAccount(Account account)
        {
            // The method saves account changes to a file
            foreach (var item in Accounts)
            {
                if (item.Name == account.Name)
                {
                    item.Balance = account.Balance;
                    item.Currency = account.Currency;
                }
            }
            string jsonString = JsonSerializer.Serialize(Accounts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Source, jsonString);
        }
        public Account GetAccount(string name)
        {
            // The method finds and returns an account by name,
            // if the account does not exist, it throws an exception
            foreach (var account in Accounts) 
            {
                if (account.Name == name){
                    return account;
                }
            }
            throw new ArgumentException("Account not found");
        }

        public void Info()
        {
            foreach (var account in Accounts)
            {
                Console.WriteLine($"{account.Name+":",-16} {account.Balance,-12} {account.Currency}");
            }
        }

        public void Deposite(string name, decimal value)
        {
                Account account = GetAccount(name);
                account.Deposite(value);
                SaveAccount(account);
        }
        public void Withdraw(string name, decimal value)
        {
            Account account = GetAccount(name);
            account.Withdraw(value);
            SaveAccount(account);
        }
    }
}
