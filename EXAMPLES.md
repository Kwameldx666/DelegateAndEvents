# 📚 Примеры использования / Code Examples

## 🔗 Делегаты и События / Delegates and Events

### 1. Создание продукта с событиями / Creating Product with Events

```csharp
using Product_;
using Price_;
using Manufacturer_;

// Создание производителя
var manufacturer = new Manufacturer("Apple", new List<Discount>());

// Создание продукта
var product = new Product(
    name: "iPhone 15",
    price: new Price { Currency = Currency.USD, Value = 999m },
    manufacturer: manufacturer,
    stock: 100
);

// Подписка на события
product.PriceChanged += (prod, oldPrice, newPrice) => 
    Console.WriteLine($"Цена {prod.Name} изменилась с {oldPrice.Value} до {newPrice.Value}");

product.StockChanged += (prod, oldStock, newStock) => 
    Console.WriteLine($"Остаток {prod.Name} изменился с {oldStock} до {newStock}");

// Изменение цены - вызовет событие
product.UpdatePrice(899m);
// Вывод: Цена iPhone 15 изменилась с 999 до 899

// Изменение остатка - вызовет событие  
product.UpdateStock(95);
// Вывод: Остаток iPhone 15 изменился с 100 до 95
```

### 2. Система скидок через делегаты / Discount System with Delegates

```csharp
using Discount_;

// Создание различных типов скидок
var discounts = new List<Discount>
{
    // Процентная скидка
    new Discount("Скидка 20%", DateTime.Now, 
        product => product.Price.Value *= 0.8m),
    
    // Фиксированная скидка
    new Discount("Скидка $50", DateTime.Now, 
        product => product.Price.Value -= 50m),
    
    // Условная скидка
    new Discount("Скидка для дорогих товаров", DateTime.Now,
        product => 
        {
            if (product.Price.Value > 500m)
                product.Price.Value *= 0.9m; // 10% скидка
        }),
    
    // Скидка на остатки
    new Discount("Распродажа остатков", DateTime.Now,
        product =>
        {
            if (product.Stock < 10)
                product.Price.Value *= 0.7m; // 30% скидка
        })
};

// Применение скидки
var selectedDiscount = discounts.First(d => d.Name == "Скидка 20%");
selectedDiscount.Apply(product);
```

### 3. Управление каталогом / Catalog Management

```csharp
using Catalog_;
using Client_;

// Создание каталога
var catalog = new Catalog(
    sellerCatalog: products,
    startDate: DateTime.Now.AddMonths(-1),
    endDate: DateTime.Now.AddMonths(6),
    discounts: new List<Discount>
    {
        new Discount("Каталожная скидка", DateTime.Now, 
            p => p.Price.Value *= 0.95m)
    }
);

// Создание клиента
var client = new Client
{
    Email = "customer@example.com",
    Currency = Currency.USD,
    FeaturedProducts = new List<Guid> { product.Id }
};

// Подписка клиента на уведомления
catalog.SubscribeClient(client);

// Подписка на события продуктов
foreach (var prod in catalog.SellerCatalog)
{
    prod.PriceChanged += client.OnPriceChanged;
    prod.StockChanged += client.OnStockChanged;
}

// Применение скидок
catalog.ApplyDiscounts();

// Изменение продукта вызовет уведомления
product.UpdatePrice(850m);

// Проверка уведомлений клиента
Console.WriteLine("Уведомления клиента:");
foreach (var message in client.Incoming)
{
    Console.WriteLine($"- {message}");
}
```

### 4. Многовалютная поддержка / Multi-Currency Support

```csharp
using Price_;

// Установка курсов валют
Price.SetRates(new Dictionary<Currency, decimal>
{
    { Currency.LEU, 1.0m },    // Базовая валюта
    { Currency.EUR, 0.9m },    // 1 LEU = 0.9 EUR
    { Currency.USD, 0.8m }     // 1 LEU = 0.8 USD
});

// Создание цен в разных валютах
var priceUSD = new Price { Currency = Currency.USD, Value = 100m };
var priceEUR = new Price { Currency = Currency.EUR, Value = 90m };
var priceLEU = new Price { Currency = Currency.LEU, Value = 120m };

// Автоматический пересчет при изменении валютного курса
Console.WriteLine($"Цена в USD: {priceUSD.Value}");
Console.WriteLine($"Цена в EUR: {priceEUR.Value}");
Console.WriteLine($"Цена в LEU: {priceLEU.Value}");
```

### 5. Расширения DateTime / DateTime Extensions

```csharp
using DateTimeExtensions_;

var startDate = new DateTime(2024, 1, 1);
var endDate = new DateTime(2024, 12, 31);
var checkDate = DateTime.Now;

// Проверка попадания в диапазон
if (checkDate.IsInRange(startDate, endDate))
{
    Console.WriteLine("Дата попадает в диапазон действия скидки");
    
    // Применение скидки
    var discount = new Discount("Новогодняя скидка", checkDate,
        product => product.Price.Value *= 0.8m);
    
    discount.Apply(product);
}
```

### 6. Полный пример использования / Complete Usage Example

```csharp
class Program
{
    static void Main()
    {
        // 1. Создание производителей с скидками
        var manufacturers = CreateManufacturers();
        
        // 2. Создание продуктов
        var products = CreateProducts(manufacturers);
        
        // 3. Создание каталога
        var catalog = CreateCatalog(products);
        
        // 4. Создание и подписка клиентов
        var clients = CreateAndSubscribeClients(catalog);
        
        // 5. Установка курсов валют
        SetCurrencyRates();
        
        // 6. Применение скидок
        ApplyDiscounts(catalog);
        
        // 7. Демонстрация работы событий
        DemonstrateEvents(products, clients);
        
        // 8. Вывод результатов
        DisplayResults(clients);
    }
    
    static List<Manufacturer> CreateManufacturers()
    {
        return new List<Manufacturer>
        {
            new Manufacturer("TechCorp", new List<Discount>
            {
                new Discount("Скидка производителя", DateTime.Now, 
                    p => p.Price.Value *= 0.9m)
            }),
            new Manufacturer("InnovateLtd", new List<Discount>
            {
                new Discount("Сезонная скидка", DateTime.Now, 
                    p => p.Price.Value *= 0.85m)
            })
        };
    }
    
    // ... остальные методы
}
```

## 🎯 Ключевые концепции / Key Concepts

### Делегаты (Delegates)
- `Action<Product>` - делегат для применения скидок
- Позволяют передавать методы как параметры
- Обеспечивают гибкость в применении различных операций

### События (Events)
- `PriceChanged` - событие изменения цены
- `StockChanged` - событие изменения остатка
- Обеспечивают слабосвязанную архитектуру
- Позволяют множественную подписку

### Event-Driven Architecture
- Разделение ответственности между компонентами
- Асинхронные уведомления
- Масштабируемость системы

## 🔧 Продвинутые сценарии / Advanced Scenarios

### Цепочка делегатов / Delegate Chaining

```csharp
// Создание цепочки скидок
Action<Product> discountChain = product => { };

discountChain += p => p.Price.Value *= 0.9m;  // 10% скидка
discountChain += p => p.Price.Value -= 5m;    // Дополнительная скидка $5
discountChain += p => Console.WriteLine($"Применена скидка к {p.Name}");

// Применение всей цепочки
discountChain(product);
```

### Условное применение скидок / Conditional Discount Application

```csharp
// Применение скидок в зависимости от условий
catalog.ApplyDiscounts(product => 
{
    // Выбор скидки в зависимости от остатка
    if (product.Stock < 10)
        return product.Manufacturer.Discounts
            .FirstOrDefault(d => d.Name.Contains("Распродажа"));
    
    // Выбор скидки в зависимости от цены
    if (product.Price.Value > 1000)
        return product.Manufacturer.Discounts
            .FirstOrDefault(d => d.Name.Contains("VIP"));
    
    return product.Manufacturer.Discounts.FirstOrDefault();
});
```

---

*Этот файл содержит практические примеры использования делегатов и событий в проекте DelegateAndEvents.*