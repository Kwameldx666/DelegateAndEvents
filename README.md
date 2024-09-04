# Delegates & Events Project

## Описание проекта

Этот проект реализует систему учета продуктов, включая работу с производителями, скидками и клиентами. Он демонстрирует использование делегатов и событий в C# для управления изменениями в каталоге продуктов, уведомлениями клиентов и применением скидок.

## Структура проекта

Проект состоит из следующих классов:

- **Discount**: Содержит информацию о скидках, включая название, дату и делегат для применения скидки на продукт.
- **Manufacturer**: Описывает производителя и его скидки.
- **Product**: Представляет продукт с уникальным идентификатором, названием, ценой, количеством на складе и производителем.
- **Price**: Управляет ценами продуктов, поддерживает курсы валют и содержит события для уведомлений о изменениях цен и количества на складе.
- **Catalog**: Представляет каталог продуктов продавца, включает методы для управления скидками и уведомления клиентов о изменениях в продукте.
- **Client**: Описывает клиента, его избранные продукты и возможности для получения уведомлений.
- **DateTimeExtensions**: Содержит расширения для `DateTime`, позволяющие проверять, попадает ли дата в заданный диапазон.

## Основные функции

1. **Управление скидками**:
   - Применение скидок как на уровне производителя, так и на уровне продавца.
   - Возможность применения конкретных скидок через делегаты.

2. **Управление каталогом**:
   - Подписка клиентов на уведомления о изменениях в продуктах.
   - Уведомление клиентов о доступности продуктов на складе.

3. **Обработка событий**:
   - События для уведомления клиентов о изменении цен и количества продуктов на складе.

4. **Расширения `DateTime`**:
   - Проверка попадания даты в диапазон для применения скидок.

## Системные требования

- .NET 6.0 или выше

## Запуск проекта

1. Клонируйте репозиторий на свой компьютер.
2. Откройте проект в Visual Studio или другом редакторе.
3. Постройте проект.
4. Запустите программу, чтобы увидеть работу системы.

---
