// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Security.Principal;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
namespace financeTracker
{
    public 
    class Program
    {
        static void Main(string[] args)
        {

            // Setting the en-US culture for the current stream
            CultureInfo culture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            // 
            string accountFile = "accounts.json";
            string transactionFile = "transactions.json";
            string rateFile = "rites.json";
            IAccountList accountList = new AccountList(accountFile);
            ITransactionList transactionList = new TransactionList(transactionFile);
            IRates rates = new Rates(rateFile);
            Session session = new Session();
            if (args.Length > 0)
            {
                // Commands that are always executed
                if (args.Length > 1)
                {
                    switch (args[0])
                    {
                        case "login":
                            // The method checks if the user is registered, then makes him active.
                            // If not registered, then registers and makes him active.
                            session.Register(args[1]);

                            // logging
                            var parObj = new { account_name = args[1] };
                            var obj = new { event_name = "user_logged_id", timestamp = DateTime.Now.ToString(), param = parObj };

                            string json = JsonSerializer.Serialize(obj);
                            session.AddLog(json);
                            break;

                    }
                }
                if (session.ActiveUser != null)
                {
                    // Commands that are executed when the user is active
                    switch (args[0])
                    {
                        case "change_plan":
                            if (args.Length > 1)
                            {
                                //The method changes the plan for the active user.
                                //The link to the list of accounts is needed
                                //to calculate the balance of all user accounts
                                try
                                {
                                    session.ChangePlan(args[1], accountList);
                                    // logging
                                    var parObj = new { user_name = session.ActiveUser.UserName, plan_name = args[1] };
                                    var obj = new { event_name = "plan_changed", timestamp = DateTime.Now.ToString(), param = parObj };

                                    string json = JsonSerializer.Serialize(obj);
                                    session.AddLog(json);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            break;
                        case "add":

                            if (args.Length > 3)
                            {
                                switch (args[1])
                                {
                                    
                                    case "account":
                                        int limit_account = 2;
                                        if (session.ActiveUser.Plan == "gold") limit_account = 10;
                                        // Добавить аккаунт в список аккантов пользователя

                                        if (session.ActiveUser.Accounts.Count < limit_account)
                                        {
                                            accountList.AddAccount(args[2], args[3]);
                                            session.AddAccount(args[2]);

                                            // logging 

                                            var parObj = new { account_name = args[2], currency = args[3] };
                                            var obj = new { event_name = "account_added", timestamp = DateTime.Now.ToString(), param = parObj };

                                            string json = JsonSerializer.Serialize(obj);
                                            //Console.WriteLine(json);
                                            session.AddLog(json);

                                            if (session.ActiveUser.Accounts.Count == limit_account)
                                            {
                                                // logging
                                                var parObj1 = new { limit_type = "accounts" };
                                                var obj1 = new { event_name = "limit_reached", timestamp = DateTime.Now.ToString(), param = parObj1 };

                                                string json1 = JsonSerializer.Serialize(obj1);
                                                session.AddLog(json1);
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("The account limit for your plan has been exceeded");
                                        }
                                        
                                        break;
                                    case "money":
                                        string comment = "";
                                        string date = DateTime.Now.ToString();

                                        if (args.Length > 4)
                                        {
                                            comment = args[4];
                                        }

                                        if (args.Length > 5)
                                        {
                                            date = args[5];
                                        }
                                        decimal limit_balance = 10000;
                                        if (session.ActiveUser.Plan == "gold") limit_balance = 1000000;
                                        string currency = accountList.GetAccount(args[2]).Currency;
                                        decimal amountUSD = session.ActiveUser.BalanceUSD(Convert.ToDecimal(args[3]),currency);
                                        if (session.ActiveUser.TotalBalance(accountList) + amountUSD <= limit_balance)
                                        {
                                            try
                                            {
                                                accountList.Deposite(args[2], Convert.ToDecimal(args[3]));
                                                Transaction transaction = new Transaction(args[0], Convert.ToDecimal(args[3]), args[2], comment, date);
                                                transactionList.AddRecord(transaction);

                                                // logging
                                                var parObj = new { account_name = args[2], amount = Convert.ToDecimal(args[3]) };
                                                var obj = new { event_name = "money_added", timestamp = DateTime.Now.ToString(), param = parObj };

                                                string json = JsonSerializer.Serialize(obj);
                                                session.AddLog(json);

                                                if (session.ActiveUser.TotalBalance(accountList) == limit_balance)
                                                {
                                                    // logging
                                                    var parObj1 = new { limit_type = "balance" };
                                                    var obj1 = new { event_name = "limit_reached", timestamp = DateTime.Now.ToString(), param = parObj1 };

                                                    string json1 = JsonSerializer.Serialize(obj1);
                                                    session.AddLog(json1);
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Balance limit exceeded.");
                                        }
                                        
                                        break;
                                }

                            }

                            break;
                        case "spend":
                            if (args.Length > 3)
                            {
                                switch (args[1])
                                {
                                    case "money":
                                        string comment = "";
                                        string date = DateTime.Now.ToString();

                                        if (args.Length > 4)
                                        {
                                            comment = args[4];
                                        }

                                        if (args.Length > 5)
                                        {
                                            date = args[5];
                                        }

                                        try
                                        {
                                            accountList.Withdraw(args[2], Convert.ToDecimal(args[3]));
                                            Transaction transaction = new Transaction(args[0], Convert.ToDecimal(args[3]), args[2], comment, date);
                                            transactionList.AddRecord(transaction);
                                            // logging
                                            var parObj = new { account_name = args[2], amount = Convert.ToDecimal(args[3]) };
                                            var obj = new { event_name = "money_spend", timestamp = DateTime.Now.ToString(), param = parObj };

                                            string json = JsonSerializer.Serialize(obj);
                                            session.AddLog(json);

                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }


                                        break;
                                }

                            }

                            break;
                        case "info":
                            if (args.Length > 1)
                            {
                                switch (args[1])
                                {
                                    case "accounts":
                                        accountList.Info();
                                        break;
                                    case "transactions":
                                        transactionList.Info();
                                        break;
                                }
                            }

                            break;
                        case "export":
                            transactionList.Export("export.txt");
                            break;
                        case "options":
                            if (args.Length > 1)
                            {
                                Actions.Options(args[1]);
                            }

                            break;
                        case "action":
                            if (args.Length > 1)
                            {
                                switch (args[1])
                                {
                                    case "info":
                                        if (args.Length > 2)
                                        {
                                            // Invoke an action Info
                                            Actions.Info(accountList.GetAccount(args[2]));
                                            // logging
                                            var parObj = new { account_name = args[2], action = args[1] };
                                            var obj = new { event_name = "account_action_invoked", timestamp = DateTime.Now.ToString(), param = parObj };

                                            string json = JsonSerializer.Serialize(obj);
                                            session.AddLog(json);
                                        }
                                        break;
                                    case "convert":
                                        if (args.Length > 3)
                                        {
                                            // Invoke an action convert
                                            try
                                            {
                                                Account account = accountList.GetAccount(args[2]);
                                                Actions.Convert(ref account, args[3], rates);
                                                accountList.SaveAccount(account);
                                                // logging
                                                var parObj = new { account_name = args[2], action = args[1] };
                                                var obj = new { event_name = "account_action_invoked", timestamp = DateTime.Now.ToString(), param = parObj };

                                                string json = JsonSerializer.Serialize(obj);
                                                session.AddLog(json);
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }



                                        }
                                        break;
                                    case "history":
                                        if (args.Length > 2)
                                        {
                                            // invoke an action History
                                            // Static method displays transaction history for the specified account
                                            // method arguments: list of account transactions, account name
                                            // method GetTransactions returns a list of transactions for the specified account.

                                            Actions.History(transactionList.GetTransactions(args[2]), args[2]);
                                            // logging
                                            var parObj = new { account_name = args[2], action = args[1] };
                                            var obj = new { event_name = "account_action_invoked", timestamp = DateTime.Now.ToString(), param = parObj };

                                            string json = JsonSerializer.Serialize(obj);
                                            session.AddLog(json);
                                        }
                                        break;

                                }
                            }
                            break;
                    }
                    

                }
                else
                {
                    if (args[0] != "login") 
                    {
                        Console.WriteLine("To work in the system you need to authenticate");
                    }
                }

                

            }
        }

    }
}


