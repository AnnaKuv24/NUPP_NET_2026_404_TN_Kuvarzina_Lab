# HotelApp — ЛР №1 (Технології на платформі .NET)

Тематика моделі: **Готель**.

## Структура

```
HotelApp/
├── HotelApp.sln
├── .gitignore
├── Hotel.Common/                       <- бібліотека класів (Class Library)
│   ├── Entities/
│   │   ├── IEntity.cs                  <- спільний контракт (Guid Id) для CRUD-сервісу
│   │   ├── Hierarchy/                  <- ІЄРАРХІЯ КЛАСІВ (є спадкування)
│   │   │   ├── Room.cs                 <- базовий (абстрактний) клас
│   │   │   ├── StandardRoom.cs         <- StandardRoom : Room
│   │   │   └── SuiteRoom.cs            <- SuiteRoom : Room
│   │   └── Objects/                    <- ІЄРАРХІЯ ОБ'ЄКТІВ (самостійні класи)
│   │       ├── Guest.cs
│   │       └── Receptionist.cs
│   ├── Delegates/
│   │   └── BookingConfirmedHandler.cs  <- делегат
│   ├── Extensions/
│   │   └── RoomExtensions.cs           <- метод розширення
│   ├── Services/
│   │   ├── ICrudService.cs             <- інтерфейс CRUD (за умовою завдання)
│   │   └── CrudService.cs              <- генерик CRUD-сервіс + Save/Load (додаткове завдання)
│   └── HotelManager.cs                 <- статичні поля/конструктор/методи, events
└── Hotel.Console/                      <- консольний застосунок (демонстрація)
    └── Program.cs
```

## Як запустити локально

```bash
cd HotelApp
dotnet build
dotnet run --project Hotel.Console
```
