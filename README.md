# 🚗 AutoService2 - Система за управление на автосервиз

Модерно кросплатформено приложение за управление на автосервизи, разработено с .NET MAUI и Blazor WebView.

## 📋 Съдържание

- [Общ преглед](#общ-преглед)
- [Функционалности](#функционалности)
- [Технологии](#технологии)
- [Изисквания](#изисквания)
- [Инсталация](#инсталация)
- [Структура на проекта](#структура-на-проекта)
- [Използване](#използване)
- [База данни](#база-данни)
- [Конфигурация](#конфигурация)

## 🎯 Общ преглед

AutoService2 е пълнофункционална система за управление на автосервизи, която предоставя интуитивен интерфейс за управление на:
- Клиенти (частни и фирмени)
- Автомобили
- Работни поръчки
- Приеми и записване на час
- Услуги и поддръжка
- Инвентар на части
- Фактуриране
- Отчети и статистики

## ✨ Функционалности

### 📊 Табло (Dashboard)
- Преглед на активни работни поръчки
- Приети днес клиенти
- Статус на части под поръчка
- Неплатени фактури
- Последни поръчки и предстоящи срокове

### 👥 Управление на клиенти
- Добавяне на частни и фирмени клиенти
- Съхраняване на контактна информация
- История на посещения
- Търсене и филтриране

### 🚙 Управление на автомобили
- Регистрация на МПС
- VIN номер и технически данни
- Километраж и история на обслужване
- Връзка с клиенти

### 🛠️ Работни поръчки
- Създаване и проследяване на поръчки
- Статуси: В изчакване, В процес, Изчакване на части, Завършена
- Приоритети: Нисък, Среден, Висок, Спешен
- Добавяне на услуги и части
- Изчисляване на общи суми

### 📅 Приеми и записване на час
- Запазване на час за обслужване
- Календарен изглед
- Избор на услуги
- Оценка на цена и времетраене

### 🔧 Каталог услуги
Пълен каталог от 13 категории с над 100 услуги:
- 🔍 Диагностика
- ⚙️ Двигател и трансмисия
- 🛑 Спирачна система
- 🔩 Ходова част и окачване
- ⚡ Електрическа система
- ❄️ Климатична система
- 🛢️ Масла и течности
- 🚗 Гуми и джанти
- ✨ Детайлинг
- 🔨 Тенекеджийство
- 🎨 Боядисване
- 🪟 Остъкляване
- 🏎️ Тунинг и аксесоари

### 📦 Инвентар
- Управление на наличност
- Минимални нива на складови наличности
- Поръчки на части
- История на движения

### 💰 Фактуриране
- Генериране на фактури
- Статуси: Чернова, Издадена, Платена, Просрочена, Анулирана
- Проследяване на плащания
- ПДВ калкулации

### 📈 Отчети
- Финансови отчети
- Статистика по услуги
- Анализ на ефективността
- Експорт на данни

### 🔧 Поддръжка и диагностика
- График на планови прегледи
- Диагностични отчети
- Автоматични напомняния
- История на поддръжката

## 🛠️ Технологии

### Framework & Платформи
- **.NET 10** - Най-новата версия на .NET
- **.NET MAUI** - Кросплатформено развитие
- **Blazor WebView** - Уеб-базиран UI
- **C# 14.0** - Програмен език

### База данни
- **SQLite** - Лека, вградена база данни
- **Entity Framework Core 9.0** - ORM

### Платформи
- ✅ Android (24.0+)
- ✅ iOS (15.0+)
- ✅ macOS Catalyst (15.0+)
- ✅ Windows (10.0.17763.0+)

### UI Framework
- **Razor Components** - Компонентна архитектура
- **CSS** - Модерен, тъмен дизайн
- Responsive дизайн

## 📋 Изисквания

### За разработка
- Visual Studio 2022 (17.13 или по-нова версия) или Visual Studio 2025
- .NET 10 SDK
- Workloads за .NET MAUI:
  - .NET Multi-platform App UI development
  - Android development
  - iOS development (само за Mac)
  - Mac Catalyst development (само за Mac)

### За стартиране
- Минимум 4 GB RAM
- 500 MB свободно дисково пространство
- Операционна система според платформата

## 🚀 Инсталация

### 1. Клониране на репозиторито
```bash
git clone https://github.com/unrealbg/AutoService2.git
cd AutoService2
```

### 2. Отваряне в Visual Studio
```bash
# Отворете AutoService2.sln във Visual Studio
start AutoService2.sln
```

### 3. Възстановяване на NuGet пакети
```bash
dotnet restore
```

### 4. Стартиране на приложението
- Изберете целева платформа (Android, iOS, Windows, Mac Catalyst)
- Натиснете F5 или Run

При първо стартиране:
- Базата данни ще бъде създадена автоматично
- Ще бъдат заредени примерни данни
- Ще бъде създаден каталог с услуги

## 📁 Структура на проекта

```
AutoService2/
├── Components/              # Blazor компоненти
│   ├── Layout/             # Layout компоненти
│   │   ├── MainLayout.razor
│   │   └── NavMenu.razor
│   ├── Pages/              # Страници
│   │   ├── Home.razor      # Табло
│   │   ├── Customers.razor # Клиенти
│   │   ├── Vehicles.razor  # Автомобили
│   │   ├── WorkOrders.razor # Поръчки
│   │   ├── Appointments.razor # Приеми
│   │   ├── Services.razor  # Услуги
│   │   ├── Inventory.razor # Инвентар
│   │   ├── Billing.razor   # Фактуриране
│   │   ├── Reports.razor   # Отчети
│   │   └── Settings.razor  # Настройки
│   └── Shared/             # Споделени компоненти
│       └── Modal.razor
├── Data/                   # Слой за данни
│   ├── AutoServiceDbContext.cs
│   ├── DatabaseInitializer.cs
│   └── Repositories/       # Repository pattern
│       ├── IRepository.cs
│       ├── Repository.cs
│       ├── VehicleRepository.cs
│       ├── CustomerRepository.cs
│       └── WorkOrderRepository.cs
├── Models/                 # Модели на данни
│   ├── Vehicle.cs
│   ├── Customer.cs
│   ├── WorkOrder.cs
│   ├── Appointment.cs
│   ├── Invoice.cs
│   ├── Part.cs
│   ├── ServiceType.cs
│   ├── MaintenanceSchedule.cs
│   └── DiagnosticReport.cs
├── Services/               # Бизнес логика
│   ├── DashboardService.cs
│   ├── VehicleService.cs
│   ├── CustomerService.cs
│   ├── WorkOrderService.cs
│   ├── ServiceTypeService.cs
│   └── MaintenanceService.cs
├── wwwroot/                # Статични ресурси
│   ├── css/
│   │   └── app.css        # Главен стил
│   └── index.html
├── Resources/              # MAUI ресурси
│   ├── AppIcon/
│   ├── Fonts/
│   ├── Images/
│   └── Splash/
└── MauiProgram.cs         # Точка на влизане
```

## 💻 Използване

### Първи стъпки

1. **Стартирайте приложението**
   - Приложението стартира с празна база данни
   - Автоматично се зарежда каталог с услуги

2. **Добавете клиент**
   - Отидете в раздел "Клиенти"
   - Кликнете "Нов клиент"
   - Попълнете данните
   - Запазете

3. **Регистрирайте автомобил**
   - Отидете в раздел "Автомобили"
   - Кликнете "Нов автомобил"
   - Изберете клиент
   - Въведете данни за МПС
   - Запазете

4. **Създайте работна поръчка**
   - Отидете в раздел "Поръчки"
   - Кликнете "Нова поръчка"
   - Изберете клиент и автомобил
   - Добавете услуги
   - Запазете

### Прием на клиент

1. Отидете в раздел "Прием"
2. Въведете данни на клиента
3. Изберете услуга от каталога
4. Изберете дата и час
5. Запазете приема

### Фактуриране

1. Завършете работна поръчка
2. Отидете в "Фактуриране"
3. Създайте нова фактура
4. Добавете услуги и части
5. Генерирайте и изпратете фактурата

## 🗄️ База данни

### Локация
База данни се съхранява в:
- **Android**: `/data/data/com.companyname.autoservice2/files/autoservice.db`
- **iOS**: `Library/autoservice.db`
- **Windows**: `%LOCALAPPDATA%\AutoService2\autoservice.db`
- **macOS**: `~/Library/Containers/com.companyname.autoservice2/Data/Library/autoservice.db`

### Схема
Основни таблици:
- `Customers` - Клиенти
- `Vehicles` - Автомобили
- `WorkOrders` - Работни поръчки
- `WorkOrderItems` - Артикули в поръчка
- `Appointments` - Приеми
- `Invoices` - Фактури
- `Parts` - Части
- `ServiceTypes` - Типове услуги
- `MaintenanceSchedules` - График на поддръжка
- `DiagnosticReports` - Диагностични отчети
- `Settings` - Настройки

### Миграции
```bash
# Създаване на нова миграция
dotnet ef migrations add MigrationName

# Прилагане на миграции
dotnet ef database update
```

### Нулиране на базата данни
За да нулирате базата данни:
1. Изтрийте файла `autoservice.db`
2. Рестартирайте приложението
3. Базата ще бъде създадена отново с примерни данни

## ⚙️ Конфигурация

### Dependency Injection
Всички сервизи са регистрирани в `MauiProgram.cs`:

```csharp
// Repositories
builder.Services.AddScoped<VehicleRepository>();
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<WorkOrderRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Services
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<VehicleService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<WorkOrderService>();
builder.Services.AddScoped<ServiceTypeService>();
builder.Services.AddScoped<MaintenanceService>();
```

### Персонализация на UI
Редактирайте `wwwroot/css/app.css` за промяна на:
- Цветове
- Шрифтове
- Размери
- Анимации

## 🤝 Принос

Приносът е добре дошъл! За да допринесете:

1. Направете Fork на проекта
2. Създайте Branch за вашата функция (`git checkout -b feature/AmazingFeature`)
3. Commit промените (`git commit -m 'Add some AmazingFeature'`)
4. Push към Branch (`git push origin feature/AmazingFeature`)
5. Отворете Pull Request

## 📝 Лиценз

Този проект е с отворен код и е достъпен под [MIT License](LICENSE).

## 👨‍💻 Автор

**unrealbg**
- GitHub: [@unrealbg](https://github.com/unrealbg)
- Repository: [AutoService2](https://github.com/unrealbg/AutoService2)

## 📧 Контакти

За въпроси и предложения:
- Отворете Issue в GitHub
- Свържете се чрез Pull Request

## 🙏 Благодарности

- Microsoft за .NET MAUI и Blazor
- Entity Framework Core екипа
- Общността на разработчици

## 📸 Екранни снимки

![Табло](https://www.unrealbg.com/autoservice/1.png "Табло")
![Прием](https://www.unrealbg.com/autoservice/2.png "Прием")
![Автомобили](https://www.unrealbg.com/autoservice/3.png "Автомобили")
![Нов автомобил](https://www.unrealbg.com/autoservice/3.1.png "Нов автомобил")
![Клиенти](https://www.unrealbg.com/autoservice/4.png "Клиенти")
![Нов клиент](https://www.unrealbg.com/autoservice/4.1.png "Нов клиент")
![Поръчки/Ремонти](https://www.unrealbg.com/autoservice/5.png "Поръчки/Ремонти")
![Склад](https://www.unrealbg.com/autoservice/6.png "Склад")
![Фактуриране](https://www.unrealbg.com/autoservice/7.png "Фактуриране")
![Отчети](https://www.unrealbg.com/autoservice/8.png "Отчети")
![Настройки](https://www.unrealbg.com/autoservice/2.png "Настройки")



## 🔮 Бъдещи планове

- [ ] Добавяне на облачна синхронизация
- [ ] Мобилно приложение за техниците
- [ ] Интеграция с платежни системи
- [ ] SMS/Email нотификации
- [ ] Интеграция с онлайн резервационни системи
- [ ] Мулти-локация поддръжка
- [ ] API за трети страни
- [ ] Разширени отчети и аналитика

---
