using Hotel.Common.Entities.Hierarchy;
using Hotel.Common.Services;
using Hotel.Common.Synchronization;
using System.Diagnostics;

namespace Hotel.ConsoleApp.Demos;

/// <summary>Демонстрація ЛР №2 (запуск: dotnet run --project Hotel.Console -- --legacy).</summary>
public static class Lab2Demo
{
    public static async Task RunAsync()
    {
        Console.WriteLine("\n\n=== ЛР №2: асинхронний CRUD, паралелізм, LINQ ===\n");

        // --- 1. Паралельне створення 1000 об'єктів через CreateNew() і Parallel ---
        var asyncRoomService = new InMemoryCrudServiceAsync<StandardRoom>("standard_rooms_async.json");

        const int roomsToGenerate = 1000;
        var stopwatch = Stopwatch.StartNew();

        Parallel.For(0, roomsToGenerate, i =>
        {
            var room = StandardRoom.CreateNew();
            asyncRoomService.CreateAsync(room).GetAwaiter().GetResult();
        });

        stopwatch.Stop();
        Console.WriteLine($"Паралельно згенеровано {roomsToGenerate} номерів за {stopwatch.ElapsedMilliseconds} мс.");

        // --- 2. LINQ: мінімальне, максимальне, середнє значення ціни ---
        var allGeneratedRooms = (await asyncRoomService.ReadAllAsync()).ToList();

        var minPrice = allGeneratedRooms.Min(r => r.PricePerNight);
        var maxPrice = allGeneratedRooms.Max(r => r.PricePerNight);
        var avgPrice = allGeneratedRooms.Average(r => r.PricePerNight);

        Console.WriteLine($"\n-- Статистика по ціні за ніч (LINQ Min/Max/Average) --");
        Console.WriteLine($"Мінімальна ціна: {minPrice:F2} грн");
        Console.WriteLine($"Максимальна ціна: {maxPrice:F2} грн");
        Console.WriteLine($"Середня ціна:    {avgPrice:F2} грн");

        // --- 3. Пагінація: показати лише першу "сторінку" з 5 номерів ---
        var firstPage = await asyncRoomService.ReadAllAsync(page: 1, amount: 5);
        Console.WriteLine("\n-- Пагінація: перші 5 номерів (сторінка 1, розмір 5) --");
        foreach (var room in firstPage)
            Console.WriteLine(room.GetInfo());

        // --- 4. Асинхронне збереження всієї згенерованої колекції у файл ---
        var saved = await asyncRoomService.SaveAsync();
        Console.WriteLine($"\nЗбережено {roomsToGenerate} номерів у файл асинхронно: {saved} " +
                           $"({Path.GetFullPath("standard_rooms_async.json")})");

        // --- 5. Приклади примітивів синхронізації ---
        Console.WriteLine("\n-- Примітиви синхронізації --");
        SynchronizationExamples.LockExample();
        await SynchronizationExamples.SemaphoreExample();
        SynchronizationExamples.AutoResetEventExample();
        SynchronizationExamples.MonitorExample();

        Console.WriteLine("\n=== Кінець демонстрації ЛР №2 ===");
    }
}