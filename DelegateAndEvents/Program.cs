using Catalog_;
using Client_;
using Discount_;
using Manufacturer_;
using Price_;
using Product_;
using System;
using System.Collections.Generic;
using System.Linq;

// Main program class
public class Program
{
    public static void Main()
    {
        // Initialize a list of manufacturers with discounts
        var manufacturers = new List<Manufacturer>
        {
            new Manufacturer("Manufacturer1", new List<Discount>
            {
                new Discount("Early Bird Discount", DateTime.Now.AddMonths(-1), p => p.Price.Value *= 0.9m), // 10% discount
                new Discount("Seasonal Discount", DateTime.Now, p => p.Price.Value *= 0.85m) // 15% discount
            }),
            new Manufacturer("Manufacturer2", new List<Discount>
            {
                new Discount("Summer Sale", DateTime.Now.AddDays(-10), p => p.Price.Value *= 0.8m), // 20% discount
                new Discount("Winter Discount", DateTime.Now.AddMonths(2), p => p.Price.Value *= 0.75m) // 25% discount
            }),
            new Manufacturer("Manufacturer3", new List<Discount>
            {
                new Discount("Clearance Sale", DateTime.Now.AddMonths(-2), p => p.Price.Value *= 0.7m), // 30% discount
                new Discount("Black Friday", DateTime.Now.AddMonths(1), p => p.Price.Value *= 0.5m) // 50% discount
            })
        };

        // Initialize a list of products
        var products = new List<Product>
        {
            new Product( "Product1", new Price { Currency = Currency.USD, Value = 100 }, manufacturers[0], 50),
            new Product( "Product2", new Price { Currency = Currency.EUR, Value = 200 }, manufacturers[1], 30),
            new Product( "Product3", new Price { Currency = Currency.LEU, Value = 300 }, manufacturers[2], 20)
        };

        // Initialize a catalog with a parameterized constructor
        var catalog = new Catalog(
            sellerCatalog: products,
            startDate: DateTime.Now.AddMonths(-1),
            endDate: DateTime.Now.AddMonths(3),
            discounts: new List<Discount>
            {
                new Discount("Catalog Discount", DateTime.Now, p => p.Price.Value *= 0.95m) // 5% discount on all catalog products
            }
        );

        // Set currency rates
        Price.SetRates(new Dictionary<Currency, decimal>
        {
            { Currency.LEU, 1.0m },
            { Currency.EUR, 0.9m },
            { Currency.USD, 0.8m }
        });

        // Initialize a list of clients
        var clients = new List<Client>
        {
            new Client
            {
                Email = "client1@example.com",
                Currency = Currency.USD,
                FeaturedProducts = catalog.SellerCatalog.Select(p => p.Id).ToList() // Adding all IDs from the catalog
            },
            new Client
            {
                Email = "client2@example.com",
                Currency = Currency.EUR,
                FeaturedProducts = new List<Guid> { Guid.NewGuid(), catalog.SellerCatalog.First().Id } // One product from the catalog and one non-existent
            }
        };

        // Subscribe clients to product changes
        foreach (var client in clients)
        {
            foreach (var product in catalog.SellerCatalog)
            {
                product.PriceChanged += client.OnPriceChanged;
                product.StockChanged += client.OnStockChanged;
            }
        }

        // Apply discounts based on the compilation mode
#if DEBUG
        catalog.ApplyDiscounts(p => p.Manufacturer.Discounts.FirstOrDefault(d => d.Name == "Seasonal Discount"));
#else
        catalog.ApplyDiscounts();
#endif
        catalog.SubscribeClient(clients[0]);
        // Update product prices
        products[0].UpdatePrice(90); // Change price
        products[1].UpdatePrice(180); // Change price


        // Example of sending a notification to a client
        bool notificationResult = clients[0].Notificate("This is a notification message.");
        Console.WriteLine($"Notification sent: {notificationResult}");

        // Display client information
        foreach (var client in clients)
        {
            Console.WriteLine($"Email: {client.Email}");

            // Display tracked products
            var trackedProducts = catalog.SellerCatalog
                .Where(p => client.FeaturedProducts.Contains(p.Id))
                .Select(p => p.Name)
                .ToList();

            if (trackedProducts.Any())
            {
                Console.WriteLine("Products: " + string.Join(", ", trackedProducts));
            }
            else
            {
                Console.WriteLine("Products: No tracked products");
            }

            // Display incoming messages
            Console.WriteLine("Inbox:");
            if (client.Incoming.Any())
            {
                foreach (var message in client.Incoming)
                {
                    Console.WriteLine(message);
                }
            }
            else
            {
                Console.WriteLine("No messages");
            }

            Console.WriteLine(); // Empty line for separating client information
        }
    }
}