using Manufacturer_;
using Price_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product_
{
    // Class representing a product
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Price Price { get; set; }
        public Manufacturer Manufacturer { get; set; }

        // Private field for stock
        private int _stock;

        // Property for stock with validation and event invocation
        public int Stock
        {
            get => _stock;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Stock must be greater than zero.");
                }
                _stock = value;
            }
        }

        // Events for price and stock changes
        public event Action<Product, Price, Price> PriceChanged;
        public event Action<Product, int, int> StockChanged;

        // Method to update stock and invoke the StockChanged event
        public void UpdateStock(int newStock)
        {
            var oldStock = Stock;
            Stock = newStock;
            StockChanged?.Invoke(this, oldStock, newStock);
        }

        // Method to update price and invoke the PriceChanged event
        public void UpdatePrice(decimal newValue)
        {
            var oldPrice = new Price
            {
                Currency = Price.Currency,
                Value = Price.Value
            };

            Price.Value = newValue;

            // Invoke the event to notify subscribers about the price change
            PriceChanged?.Invoke(this, oldPrice, Price);
        }

        // Constructor to initialize a product
        public Product(string name, Price price, Manufacturer manufacturer, int stock)
        {
            Name = name;
            Price = price;
            Id = Guid.NewGuid();
            Manufacturer = manufacturer;
            Stock = stock;
        }
    }

}
