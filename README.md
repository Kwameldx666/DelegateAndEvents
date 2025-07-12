# 🎯 Delegates & Events Project

<div align="center">

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)
![Build](https://img.shields.io/badge/Build-Passing-brightgreen?style=for-the-badge)

**🔗 Демонстрация делегатов и событий в C# через систему управления каталогом продуктов**

[English](#english) | [Русский](#русский)

</div>

---

## 📑 Содержание

- [🎯 О проекте](#-о-проекте)
- [✨ Ключевые особенности](#-ключевые-особенности)
- [🏗️ Архитектура](#️-архитектура)
- [🚀 Быстрый старт](#-быстрый-старт)
- [📖 Примеры использования](#-примеры-использования)
- [📚 Подробные примеры кода](EXAMPLES.md)
- [🔧 API](#-api)
- [📊 Пример вывода](#-пример-вывода)
- [🤝 Вклад в проект](#-вклад-в-проект)
- [📄 Лицензия](#-лицензия)

---

## 🎯 О проекте

Этот проект представляет собой **учебную демонстрацию** мощных возможностей **делегатов и событий** в C#. Реализована комплексная система управления каталогом продуктов с поддержкой:

- 🛒 **Продуктов** с многовалютными ценами
- 🏭 **Производителей** со своими системами скидок  
- 👥 **Клиентов** с персонализированными уведомлениями
- 💰 **Скидок** через делегаты
- 📡 **Event-driven архитектуры** для уведомлений в реальном времени

## ✨ Ключевые особенности

### 🔗 Делегаты и События
- **Делегаты** для применения скидок (`Action<Product>`)
- **События** для уведомлений о изменении цен и остатков
- **Event-driven** архитектура для слабосвязанной системы

### 💱 Многовалютная поддержка
- Поддержка валют: **USD**, **EUR**, **LEU**
- Автоматический пересчет по курсам валют
- Клиентские настройки валют

### 🎁 Система скидок
- Скидки от **производителей**
- Скидки от **продавцов** 
- Временные рамки действия скидок
- Гибкое применение через делегаты

### 📢 Уведомления
- Подписка клиентов на избранные продукты
- Уведомления об изменении цен
- Уведомления об изменении остатков

## 🏗️ Архитектура

```
┌─────────────────┐    ┌──────────────────┐    ┌─────────────────┐
│    Client       │    │     Catalog      │    │   Manufacturer  │
│                 │    │                  │    │                 │
│ • Email         │◄──►│ • Products       │◄──►│ • Name          │
│ • Currency      │    │ • Discounts      │    │ • Discounts     │
│ • Featured      │    │ • Subscribers    │    │                 │
│ • Notifications │    │                  │    │                 │
└─────────────────┘    └──────────────────┘    └─────────────────┘
         ▲                       ▲                       │
         │                       │                       │
         │              ┌────────▼────────┐              │
         │              │     Product     │◄─────────────┘
         │              │                 │
         └──────────────┤ • Name          │
           Events       │ • Price         │
                        │ • Stock         │
                        │ • Events        │
                        └─────────────────┘
```

### 📁 Структура классов

| Класс | Назначение | Ключевые возможности |
|-------|------------|---------------------|
| **🛍️ Product** | Продукт | События `PriceChanged`, `StockChanged` |
| **🏭 Manufacturer** | Производитель | Коллекция скидок |
| **💰 Price** | Цена | Поддержка валют и курсов |
| **🎁 Discount** | Скидка | Делегат `Action<Product>` для применения |
| **📋 Catalog** | Каталог | Управление продуктами и подписчиками |
| **👤 Client** | Клиент | Избранные продукты, уведомления |
| **📅 DateTimeExtensions** | Расширения | Проверка дат для скидок |

## 🚀 Быстрый старт

### ⚡ Системные требования

- ![.NET](https://img.shields.io/badge/.NET-8.0+-512BD4?style=flat-square&logo=dotnet) **.NET 8.0** или выше
- ![IDE](https://img.shields.io/badge/IDE-Visual%20Studio%20|%20VS%20Code%20|%20Rider-blue?style=flat-square) Любая IDE с поддержкой C#

### 📥 Установка

```bash
# Клонируйте репозиторий
git clone https://github.com/Kwameldx666/DelegateAndEvents.git

# Перейдите в директорию проекта
cd DelegateAndEvents

# Восстановите зависимости
dotnet restore

# Соберите проект
dotnet build

# Запустите приложение
dotnet run --project DelegateAndEvents
```

## 📖 Примеры использования

### 🎁 Создание скидки через делегат

```csharp
// Создание скидки 15% через делегат
var seasonalDiscount = new Discount(
    "Сезонная скидка", 
    DateTime.Now, 
    product => product.Price.Value *= 0.85m // 15% скидка
);

// Применение к производителю
manufacturer.Discounts.Add(seasonalDiscount);
```

### 📡 Подписка на события продукта

```csharp
// Подписка клиента на изменения продукта
product.PriceChanged += client.OnPriceChanged;
product.StockChanged += client.OnStockChanged;

// Изменение цены вызовет событие
product.UpdatePrice(95.50m);
```

### 🏪 Управление каталогом

```csharp
// Создание каталога с продуктами и скидками
var catalog = new Catalog(
    sellerCatalog: products,
    startDate: DateTime.Now.AddMonths(-1),
    endDate: DateTime.Now.AddMonths(3),
    discounts: catalogDiscounts
);

// Подписка клиента на уведомления
catalog.SubscribeClient(client);

// Применение скидок
catalog.ApplyDiscounts();
```

## 🔧 API

### 📱 Основные методы

#### Product
```csharp
void UpdatePrice(decimal newValue)     // Обновление цены с событием
void UpdateStock(int newStock)        // Обновление остатка с событием

// События
event Action<Product, Price, Price> PriceChanged;
event Action<Product, int, int> StockChanged;
```

#### Catalog
```csharp
void SubscribeClient(Client client)              // Подписка клиента
void ApplyDiscounts()                           // Применение всех скидок
void ApplyDiscounts(Func<Product, Discount> selector) // Выборочное применение
```

#### Client
```csharp
void OnPriceChanged(Product product, Price oldPrice, Price newPrice)
void OnStockChanged(Product product, int oldStock, int newStock)
bool Notificate(string message)                // Отправка уведомления
```

## 📊 Пример вывода

```
Notification sent: True
Email: client1@example.com
Products: Product1, Product2, Product3
Inbox:
Product 'Product1' has a new price: 90 USD
The price of the product Product1 changed from 100 USD to 90 USD.
Product 'Product2' has a new price: 180 EUR
The price of the product Product2 changed from 177.78 USD to 160 USD.
This is a notification message.

Email: client2@example.com
Products: Product1
Inbox:
Product 'Product1' has a new price: 90 USD
Product 'Product2' has a new price: 180 EUR
```

---

# English

## 🎯 About

This project is an **educational demonstration** of **delegates and events** in C#. It implements a comprehensive product catalog management system featuring real-time notifications, multi-currency support, and event-driven architecture.

## ✨ Key Features

- 🔗 **Delegates & Events**: Advanced usage of `Action<Product>` delegates and custom events
- 💱 **Multi-Currency**: Support for USD, EUR, LEU with automatic conversion
- 🎁 **Dynamic Discounts**: Flexible discount system using delegates
- 📡 **Event-Driven**: Real-time notifications for price and stock changes
- 👥 **Client Management**: Personalized product tracking and notifications

## 🚀 Quick Start

```bash
git clone https://github.com/Kwameldx666/DelegateAndEvents.git
cd DelegateAndEvents
dotnet run --project DelegateAndEvents
```

📚 **[View detailed code examples →](EXAMPLES.md)**

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details.

---

<div align="center">

**⭐ Поставьте звезду, если проект был полезен!**

Made with ❤️ for learning C# delegates and events

</div>
