using Hotel.Common;
using Hotel.Common.Entities.Hierarchy;
using Hotel.Common.Entities.Objects;
using Hotel.Common.Extensions;
using Hotel.Common.Services;

namespace Hotel.ConsoleApp.Demos;

/// <summary>Демонстрація ЛР 1 (запуск: dotnet run --project Hotel.Console -- --legacy).</summary>
public static class Lab1Demo
{
    public static void Run()
    {
        Console.WriteLine("=== Демонстрація CRUD-сервісу та моделі \"Готель\" ===\n");

        // --- 1. Створюємо CRUD-сервіси для стандартних номерів і люксів ---
        var standardRoomService = new CrudService<StandardRoom>();
        var suiteRoomService = new CrudService<SuiteRoom>();

        var room1 = new StandardRoom(101, 1200m, 2, true, "у двір");
        var room2 = new StandardRoom(102, 1350m, 2, true, "на море");
        var suite1 = new SuiteRoom(201, 4200m, 3, true, "преміум");

        // Create
        standardRoomService.Create(room1);
        standardRoomService.Create(room2);
        suiteRoomService.Create(suite1);

        Console.WriteLine("-- Усі стандартні номери після Create --");
        foreach (var r in standardRoomService.ReadAll())
            Console.WriteLine(r.GetInfo());

        // Read
        Console.WriteLine("\n-- Read одного елемента за Id --");
        var found = standardRoomService.Read(room1.Id);
        Console.WriteLine(found.GetInfo());

        // Update
        Console.WriteLine("\n-- Update --");
        room2.PricePerNight = 1500m;
        standardRoomService.Update(room2);
        Console.WriteLine(standardRoomService.Read(room2.Id).GetInfo());

        // Remove
        Console.WriteLine("\n-- Remove кімнати room1 --");
        standardRoomService.Remove(room1);
        Console.WriteLine("Залишилось стандартних номерів: " + standardRoomService.ReadAll().Count());

        // --- 2. Демонстрація extension method ---
        Console.WriteLine("\n-- Extension method ToShortString() --");
        Console.WriteLine(suite1.ToShortString());

        // --- 3. Демонстрація подій/делегатів через HotelManager ---
        Console.WriteLine("\n-- Події (events) --");
        var manager = HotelManager.CreateDefault();
        var receptionist = Receptionist.CreateSystemReceptionist();
        manager.RegisterReceptionist(receptionist);

        var guest = new Guest("Олена Коваленко", "АА123456");
        guest.BookRoom(room2.Id);
        receptionist.ConfirmBooking(room2.Id, room2.ToShortString());

        // --- 4. Статичне поле Room.TotalRoomsCount ---
        Console.WriteLine($"\nВсього створено Room (StandardRoom+SuiteRoom) за час роботи: {Room.TotalRoomsCount}");

        // --- 5. Додаткове завдання: Save / Load ---
        const string filePath = "rooms.json";
        standardRoomService.Save(filePath);
        Console.WriteLine($"\nДані збережено у файл: {Path.GetFullPath(filePath)}");

        var restoredService = new CrudService<StandardRoom>();
        restoredService.Load(filePath);
        Console.WriteLine("-- Дані, завантажені з файлу --");
        foreach (var r in restoredService.ReadAll())
            Console.WriteLine(r.GetInfo());

        Console.WriteLine("\n=== Кінець демонстрації ===");
    }
}