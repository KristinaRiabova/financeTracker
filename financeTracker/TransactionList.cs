using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace financeTracker
{
    internal class TransactionList : ITransactionList
    {
        public string FileName {get; private set;}
        public List<Transaction> Transactions { get; private set;}
        public TransactionList(string fileName) 
        {
            FileName = fileName;
            if (!File.Exists(FileName))
            {
                File.WriteAllText(FileName,"[]");
            }
            // Read Account with file Source

            try 
            {
                string jsonString = File.ReadAllText(FileName);
                Transactions = JsonSerializer.Deserialize<List<Transaction>>(jsonString) ?? new List<Transaction>();                

            }
            catch (ArgumentNullException ex)
            {

                Console.WriteLine(ex.Message);
                throw new ArgumentException("Argument Null Exception");
            }
            catch (JsonException ex)
            {
                Console.WriteLine(ex.Message);
                throw new ArgumentException("JSON Exception");
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine(ex.Message);
                throw new ArgumentException("Not supported Exception");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("File not found");
            }
            

            
        }

        public List<Transaction> GetTransactions(string accountName)
        {
            // The method returns a list of transactions for the specified account
            List<Transaction> transactions = new List<Transaction>();

            foreach (var item in Transactions)
            {
                if (item.AccountName == accountName)
                {
                    transactions.Add(new Transaction(item.Action, item.Amount, item.AccountName, item.Comment, item.Date));
                }
            }
            return transactions;
        }

        public void AddRecord(Transaction transaction)
        {
            Transactions.Add(transaction);
            string jsonString = JsonSerializer.Serialize(Transactions, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FileName, jsonString);
        }
        public void Export(string exportFile)
        {
            using (StreamWriter writer = new StreamWriter(exportFile))
            {
               
                foreach (var record in Transactions)
                {
                    writer.WriteLine(record.ToString());
                }
            }
        }

        public void Info()
        {
            foreach (var item in Transactions)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }
}
