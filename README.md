# 📦 Warehouse Management System (Система управления складом)

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4?style=flat&logo=dotnet&logoColor=white)](https://learn.microsoft.com/aspnet/core/)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-8.0-512BD4?style=flat)](https://learn.microsoft.com/ef/core/)
[![Microsoft SQL Server](https://img.shields.io/badge/Database-SQL%20Server-CC292B?style=flat&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server/)
[![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=flat&logo=bootstrap&logoColor=white)](https://getbootstrap.com/)
[![Docker](https://img.shields.io/badge/Docker-Supported-2496ED?style=flat&logo=docker&logoColor=white)](https://www.docker.com/)

Современное веб-приложение для автоматизации складского учёта, управления товарными запасами и контроля движений продукции, разработанное на стеке **ASP.NET Core 8 MVC** и **Entity Framework Core**.

---

## 🌟 Ключевые возможности

### 📦 Управление каталогом товаров (CRUD)
* **Каталог товаров:** просмотр полного перечня складских запасов с ценами, остатками и датой последнего изменения.
* **Добавление товаров:** удобная форма создания новой номенклатуры с комплексной валидацией данных.
* **Инлайн-редактирование:** возможность изменять название, описание и цену товара прямо в таблице без перехода на другие страницы.
* **Удаление:** быстрое удаление позиций с подтверждением.
* **Контроль уникальности:** защита от дублирования наименований товаров на уровне схемы БД (Unique Index).

### 🔍 Поиск, сортировка и пагинация
* **Полнотекстовый поиск:** фильтрация позиций по частичному совпадению в названии или описании товара.
* **Двунаправленная сортировка:** сортировка по возрастанию и убыванию для всех ключевых полей (наименование, цена, количество, дата изменения).
* **Серверная пагинация:** высокая производительность при больших объемах данных (по 10 позиций на страницу) с сохранением состояния поиска и сортировки.

### 📥 / 📤 Складские операции (Приход и Расход)
* **Приход товара (Импорт):** быстрое пополнение остатков через контекстное модальное окно или форму ввода.
* **Расход товара (Экспорт):** оформление списания и выдачи продукции со склада.
* **Контроль остатков:** валидация на клиенте и сервере, исключающая списание количества большего, чем имеется на складе.

### 📜 История операций (Аудит-лог)
* **Журналирование:** автоматическая фиксация каждого прихода и расхода с точной датой и временем.
* **Наглядные индикаторы:** бейджи типов операций (зеленый — приход, красный — расход) и отображение изменений количества.

### 🎨 Премиальный пользовательский интерфейс
* Стильный светлый дизайн в палитре **Slate & Indigo**.
* Полупрозрачный эффект панели навигации (**Glassmorphism** / Blur).
* Микро-анимации появления элементов и взаимодействия с карточками.
* Полная адаптивность под мобильные устройства, планшеты и мониторы высокого разрешения.

---

## 🛠 Технологический стек

* **Бэкенд:** C# 12, .NET 8.0, ASP.NET Core MVC
* **База данных и ORM:** Microsoft SQL Server, Entity Framework Core 8.0 (Code-First)
* **Миграции:** Автоматическое применение миграций при первом запуске приложения (`context.Database.Migrate()`)
* **Фронтенд:** Razor Views (CSHTML), HTML5, CSS3, JavaScript
* **Библиотеки стилей:** Bootstrap 5.3, Bootstrap Icons, кастомная дизайн-система `site.css`
* **Клиентские скрипты:** jQuery, jQuery Validation & Unobtrusive Validation
* **Контейнеризация:** Docker (многоэтапный `Dockerfile`)

---

## 📁 Структура проекта

```text
├── ASP/                                # Основной проект ASP.NET Core
│   ├── Controllers/                    # Контроллеры MVC
│   │   ├── HomeController.cs           # Информационные страницы
│   │   └── ProductController.cs        # Логика товаров и складских операций
│   ├── Models/                         # Доменные модели и DTO
│   │   ├── Item.cs                     # Модель товара склада
│   │   ├── ItemOperation.cs            # Модель лога операций (приход/расход)
│   │   └── ErrorViewModel.cs           # Модель отображения ошибок
│   ├── Services/                       # Сервисы и контекст БД
│   │   └── ProductContext.cs           # EF Core DbContext
│   ├── Migrations/                     # Файлы миграций базы данных
│   ├── Views/                          # Представления Razor
│   │   ├── Product/                    # Страницы склада (Index, NewItem, Operations)
│   │   ├── Shared/                     # Общие шаблоны (_Layout, Partials)
│   │   └── _ViewStart.cshtml
│   ├── wwwroot/                        # Статические ресурсы
│   │   ├── css/site.css                # Глобальные стили приложения
│   │   ├── js/site.js                  # Клиентские скрипты
│   │   └── lib/                        # Локальные библиотеки (Bootstrap, jQuery)
│   ├── appsettings.json                # Конфигурация (строка подключения к БД)
│   ├── Program.cs                      # Точка входа и настройка DI / Middleware
│   ├── Dockerfile                      # Скрипт сборки Docker-образа
│   └── ASP.csproj                      # Файл проекта .NET
├── ASP.sln                             # Решение Visual Studio
├── .dockerignore
└── README.md                           # Документация проекта
```

---

## 🚀 Быстрый старт

### Требования
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) или новее.
* [Microsoft SQL Server](https://www.microsoft.com/sql-server/) (Express, LocalDB или Docker-контейнер с SQL Server).
* [Visual Studio 2022](https://visualstudio.microsoft.com/) (с рабочей нагрузкой ASP.NET и веб-разработка) или [VS Code](https://code.visualstudio.com/) / [JetBrains Rider].

---

### 1. Клонирование репозитория
```bash
git clone https://github.com/your-username/warehouse-asp.git
cd warehouse-asp
```

### 2. Настройка подключения к базе данных
Откройте файл `ASP/appsettings.json` и укажите параметры подключения к вашему экземпляру SQL Server:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ProductsDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> **Примечание:** База данных `ProductsDB` и все необходимые таблицы будут созданы **автоматически** при первом запуске приложения благодаря встроенному механизму автомиграции в `Program.cs`.

---

### 3. Запуск приложения

#### Через .NET CLI:
```bash
cd ASP
dotnet run --launch-profile http
```
После запуска приложение будет доступно по адресу: **`http://localhost:5133`** (или `https://localhost:7163`).

#### Через Visual Studio 2022:
1. Откройте файл `ASP.sln`.
2. В выпадающем списке профилей запуска выберите **`http`** или **`https`**.
3. Нажмите клавишу **F5** (или `Ctrl + F5` для запуска без отладки).

---

## 🐳 Запуск в Docker

Для запуска приложения в изолированном контейнере:

```bash
# Сборка образа
docker build -t warehouse-system -f ASP/Dockerfile .

# Запуск контейнера
docker run -d -p 8080:8080 --name warehouse-app warehouse-system
```
После запуска откройте в браузере: `http://localhost:8080`.

---

## 📊 Маршруты и страницы приложения

| Маршрут | Описание |
| :--- | :--- |
| `/` или `/Product/Index` | Главная страница со списком товаров, формами прихода/расхода, поиском и сортировкой |
| `/Product/NewItem` | Страница создания и оприходования новой номенклатуры |
| `/Product/Operations` | Журнал истории складских операций (импорт/экспорт) |
| `/Product/ImportItem` | `POST`-обработчик пополнения остатка товара |
| `/Product/ExportItem` | `POST`-обработчик списания/выдачи товара со склада |
| `/Product/EditItem` | `POST`-обработчик сохранения отредактированного товара |
| `/Product/DeleteItem` | `POST`-обработчик удаления товара со склада |

---

