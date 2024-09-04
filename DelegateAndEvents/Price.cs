using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace Price_
{

    // Enum representing different currencies
    public enum Currency
    {
        LEU,
        EUR,
        USD
    }

    // Class representing a price with currency conversion functionality
    public class Price
    {
        public Currency Currency { get; set; }
        public decimal Value { get; set; }

        // Static dictionary to store currency rates
        private static readonly Dictionary<Currency, decimal> Rates = new Dictionary<Currency, decimal>
    {
        { Currency.LEU, 1.0m },
        { Currency.EUR, 19.06m },
        { Currency.USD, 17.65m }
    };

        // Method to get the rate for a given currency
        public static decimal GetRate(Currency currency)
        {
            return Rates.TryGetValue(currency, out var rate) ? rate : 1.0m;
        }

        // Method to set currency rates
        public static void SetRates(Dictionary<Currency, decimal> rates)
        {
            Rates.Clear();
            foreach (var rate in rates)
            {
                Rates[rate.Key] = rate.Value;
            }
        }
    }

}
