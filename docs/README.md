# Sabatex Core Libraries

Цей репозиторій містить набір базових бібліотек Sabatex, що забезпечують функціональність для обміну об'єктами, роботи з даними та інтеграції з різними системами.

[![MIT License](https://img.shields.io/badge/license-MIT-red.svg)](https://github.com/sabatex/Core/blob/master/LICENSE.TXT)

## Структура проекту

### Основні бібліотеки

#### 📦 Sabatex.Core
Базова бібліотека з загальними визначеннями та утилітами для всіх інших бібліотек Sabatex.

**Основні можливості:**
- Observable об'єкти з реалізованим `INotifyPropertyChanged`
- Розширення для роботи з датами (BeginOfDay, EndOfMonth, Quarter тощо)
- Розширення для роботи з Enum (Description, DisplayName)
- Розширення для роботи з символами (перекодування клавіатури)
- Клас Period для роботи з періодами дат

[Детальна документація →](../src/Sabatex.Core/README.md)

### Бібліотеки обміну об'єктами (ObjectExchange)

#### 📦 Sabatex.ObjectExchange.Core
Ядро системи обміну об'єктами між різними системами.

**Основні можливості:**
- Базова функціональність для обміну об'єктами
- Мапінг та серіалізація DTO
- Утиліти для клонування об'єктів
- Рефлексія та робота з метаданими

#### 📦 Sabatex.ObjectExchange.Models
Моделі даних для API обміну об'єктами.

**Основні можливості:**
- Моделі повідомлень (ObjectExchange, QueryObject)
- Моделі токенів та автентифікації
- Моделі дескрипторів вузлів обміну
- Підтримка версій API (v0, v1, v2)

#### 📦 Sabatex.ObjectExchange.ClientDataAdapter
Клієнтський адаптер для роботи з даними через Entity Framework.

**Основні можливості:**
- Інтеграція з Entity Framework Core
- Управління вузлами обміну
- Обробка вхідних та вихідних повідомлень
- Робота з нерозпізнаними об'єктами

#### 📦 Sabatex.ObjectExchange.ClientDataAdapter.Memory
Реалізація адаптера даних з використанням InMemory бази даних.

**Основні можливості:**
- Швидке тестування та розробка
- Не потребує налаштування БД
- Повна сумісність з ClientDataAdapter

#### 📦 Sabatex.ObjectsExchange.ApiAdapter
Адаптер для роботи з API серверу обміну об'єктами.

**Основні можливості:**
- Автентифікація клієнтів (Bearer Token)
- Відправка об'єктів на сервер
- Отримання об'єктів з сервера
- Підтримка версій API (v0, v1, v2)

#### 📦 Sabatex.ObjectsExchange.ApiAdapter.Framework
Адаптер для .NET Framework 3.5 - забезпечує сумісність зі старими системами.

**Особливості:**
- Підтримка .NET Framework 3.5
- Обмежена функціональність для сумісності
- Використання Newtonsoft.Json

#### 📦 Sabatex.ObjectsExchange.HttpClientConnector
Конектор для підключення до сервісу обміну через HTTP.

**Основні можливості:**
- HTTP клієнт для роботи з API
- Управління з'єднаннями
- Обробка помилок мережі

## Документація API

### Версії API

- [API v0 (застаріла)](Readme%20v0.md) - перша версія API
- [API v1](v1.md) - поточна стабільна версія
- [API v2](v0.md) - розширена версія з додатковою функціональністю

### Інтеграція

- [Підготовка ERP системи для обміну](Prepare%20ERP%20for%20exchange.md)

## Використання

### Встановлення пакетів

```bash
# Базова бібліотека
dotnet add package Sabatex.Core

# Клієнтська бібліотека обміну
dotnet add package Sabatex.ObjectExchange.ClientDataAdapter

# API адаптер
dotnet add package Sabatex.ObjectsExchange.ApiAdapter
```

### Швидкий старт

```csharp
// Приклад використання ClientDataAdapter
services.AddDbContext<MyDbContext>(options => 
	options.UseSqlServer(connectionString));

services.AddScoped<IClientExchangeDataAdapter, ClientExchangeDataAdapter>();

// Приклад роботи з ApiAdapter
var apiAdapter = new ExchangeApiAdapterV2(httpClient, options);
await apiAdapter.LoginAsync(clientId, password);
await apiAdapter.SendObjectAsync(destinationId, messageHeader, data);
```

## Архітектура

Система побудована на принципах:
- **Модульність** - кожна бібліотека має чітку відповідальність
- **Розширюваність** - легко додавати нові адаптери та конектори
- **Сумісність** - підтримка різних версій .NET (від 3.5 до 10.0)
- **Централізоване керування** - версії сторонніх пакетів керуються через Directory.Packages.props

## Debug vs Release

Проект використовує спеціальну систему залежностей:

- **Debug режим**: Всі бібліотеки Sabatex лінкуються через `ProjectReference`
- **Release режим**: Всі бібліотеки Sabatex використовують `PackageReference` з NuGet

Це дозволяє зручно розробляти та тестувати зміни локально, а для продакшн використовувати стабільні версії з NuGet.

## Вимоги

- .NET 10.0 (основні бібліотеки)
- .NET Framework 3.5 (ApiAdapter.Framework)
- Entity Framework Core 10.0+ (ClientDataAdapter)

## Ліцензія

Всі бібліотеки розповсюджуються під ліцензією MIT.

## Автор

**Serhiy Lakas**  
GitHub: [github.com/sabatex](https://github.com/sabatex)

## Посилання

- [GitHub Repository](https://github.com/sabatex/Core)
- [NuGet Packages](https://www.nuget.org/profiles/sabatex)
- [Sabatex ObjectsExchange (legacy)](https://github.com/sabatex/SabatexObjectsExcange)
