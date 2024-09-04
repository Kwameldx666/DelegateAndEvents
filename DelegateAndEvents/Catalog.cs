using Product_;
using Discount_;
using Client_;
using Price_;
using DateTimeExtensions_;
using Manufacturer_;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Catalog_
{
    public class Catalog
    {
        public List<Product> SellerCatalog { get; set; } = new List<Product>();
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
        public List<Discount> Discounts { get; set; } = new List<Discount>();
        private List<Client> subscribers = new List<Client>();

        // Constructor to initialize the catalog
        public Catalog(List<Product> sellerCatalog, DateTime? startDate, DateTime? endDate, List<Discount> discounts)
        {
            SellerCatalog = sellerCatalog;
            StartDate = startDate;
            EndDate = endDate;
            Discounts = discounts;

            foreach (var product in SellerCatalog)
            {
                // Subscribe to StockChanged event for each product
                product.StockChanged += (prod, oldStock, newStock) =>
                {
                    NotifyClientsOnRestock(product, oldStock, newStock);
                };
            }
        }

        // Method to subscribe a client to price change notifications
        public void SubscribeClient(Client client)
        {
            if (subscribers.Contains(client)) return;

            subscribers.Add(client);

            foreach (var product in SellerCatalog)
            {
                if (client.FeaturedProducts.Contains(product.Id))
                {
                    product.PriceChanged += (prod, oldPrice, newPrice) =>
                    {
                        var oldPriceConverted = oldPrice.Value * Price.GetRate(client.Currency) / Price.GetRate(oldPrice.Currency);
                        var newPriceConverted = newPrice.Value * Price.GetRate(client.Currency) / Price.GetRate(newPrice.Currency);

                        string message = $"The price of the product {prod.Name} changed from {oldPriceConverted} {client.Currency} to {newPriceConverted} {client.Currency}.";

                        client.Notificate(message);
                    };
                }
            }
        }

        // Method to unsubscribe a client from price change notifications
        public void UnsubscribeClient(Client client)
        {
            if (!subscribers.Contains(client)) return;

            subscribers.Remove(client);

            foreach (var product in SellerCatalog)
            {
                if (client.FeaturedProducts.Contains(product.Id))
                {
                    product.PriceChanged -= (prod, oldPrice, newPrice) =>
                    {
                        var oldPriceConverted = oldPrice.Value * Price.GetRate(client.Currency) / Price.GetRate(oldPrice.Currency);
                        var newPriceConverted = newPrice.Value * Price.GetRate(client.Currency) / Price.GetRate(newPrice.Currency);

                        string message = $"Цена продукта {prod.Name} изменилась с {oldPriceConverted} {client.Currency} на {newPriceConverted} {client.Currency}.";
                        client.Notificate(message);
                    };
                }
            }
        }

        // Notify clients when a product is restocked
        private void NotifyClientsOnRestock(Product product, int oldStock, int newStock)
        {
            if (oldStock == 0 && newStock > 0)
            {
                foreach (var client in subscribers)
                {
                    if (client.FeaturedProducts.Contains(product.Id))
                    {
                        try
                        {
                            client.Notificate($"Продукт {product.Name} снова в наличии!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при уведомлении клиента {client.Email}: {ex.Message}");
                        }
                    }
                }
            }
        }

        // Apply discounts to products
        public IEnumerable<Product> ApplyDiscounts(Func<Product, Discount> discountSelector = null)
        {
            var productsWithManufacturerDiscounts = ApplyManufacturerDiscounts(discountSelector);

            foreach (var product in productsWithManufacturerDiscounts)
            {
                foreach (var discount in Discounts)
                {
                    if (discount.Date.IsWithinRange(StartDate, EndDate))
                    {
                        discount.Apply(product);
                    }
                }

                if (product.Stock == 0 && ConvertToEUR(product.Price) < 10)
                {
                    product.UpdateStock(100);
                }
                yield return product;
            }
        }

        // Apply manufacturer discounts
        private IEnumerable<Product> ApplyManufacturerDiscounts(Func<Product, Discount> discountSelector = null)
        {
            foreach (var product in SellerCatalog)
            {
                if (discountSelector != null)
                {
                    var discount = discountSelector(product);
                    if (discount != null && discount.Date.IsWithinRange(StartDate, EndDate))
                    {
                        discount.Apply(product);
                    }
                }
                else
                {
                    foreach (var discount in product.Manufacturer.Discounts)
                    {
                        if (discount.Date.IsWithinRange(StartDate, EndDate))
                        {
                            discount.Apply(product);
                        }
                    }
                }

                foreach (var discount in Discounts)
                {
                    if (discount.Date.IsWithinRange(StartDate, EndDate))
                    {
                        discount.Apply(product);
                    }
                }

                if (product.Stock == 0 && ConvertToEUR(product.Price) < 10)
                {
                    product.UpdateStock(100);
                }

                yield return product;
            }
        }

        // Convert product price to EUR
        private decimal ConvertToEUR(Price price)
        {
            decimal rateToEUR = Price.GetRate(Currency.EUR) / Price.GetRate(price.Currency);
            return price.Value * rateToEUR;
        }
    }
}
