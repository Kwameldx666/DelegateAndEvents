
using Price_;
using Product_;
using System;
    using System.Collections.Generic;


namespace Client_
{

    // Class representing a client
    public class Client
    {
        public string Email { get; set; }
        public Currency Currency { get; set; }
        public List<Guid> FeaturedProducts { get; set; } = new List<Guid>();
        public List<string> Incoming { get; set; } = new List<string>();

        // Event handler for price changes
        public void OnPriceChanged(Product product, Price oldPrice, Price newPrice)
        {
            var message = $"Product '{product.Name}' has a new price: {newPrice.Value} {newPrice.Currency}";
            Incoming.Add(message);
        }

        // Event handler for stock changes
        public void OnStockChanged(Product product, int oldStock, int newStock)
        {
            var message = $"Product '{product.Name}' stock changed from {oldStock} to {newStock}";
            Incoming.Add(message);
        }

        // Method to notify client of product restock
        public void OnProductRestocked(Product product)
        {
            if (FeaturedProducts.Contains(product.Id))
            {
                var message = $"Product '{product.Name}' is back in stock!";
                Incoming.Add(message);
            }
        }

        // Method to send a notification to the client
        public bool Notificate(string message)
        {
            // Simulate sending a notification (always returns true in this example)
            Incoming.Add(message);
            return true;
        }
    }

}
