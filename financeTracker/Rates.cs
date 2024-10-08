using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace financeTracker
{
    internal class Rates : IRates
    {
        public List<Rate> RatesList { get; private set; }
        public Rates(string fileName) 
        { 
            if (!File.Exists(fileName))
            {
                File.WriteAllText(fileName,"[]");
            }
            try
            {
                string jsonString = File.ReadAllText(fileName);
                RatesList = JsonSerializer.Deserialize<List<Rate>>(jsonString) ?? new List<Rate>();
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
        public Rate GetRate(string currency)
        {
            foreach (Rate rate in RatesList)
            {
                if (rate.CurrencyName == currency)
                {
                    return rate;
                }
            }
            throw new ArgumentException("Unknown currency indicated");
        }
    }
}
