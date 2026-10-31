using Hotel.Common.Services;
using Hotel.Nosql;
using Hotel.Nosql.Documents;
using Hotel.Nosql.Repositories;

namespace Hotel.ConsoleApp.Demos;
public static class NosqlDemo
{
    public static async Task RunAsync()
    {
        Console.WriteLine("\n-- ДОДАТКОВЕ ЗАВДАННЯ: MongoDB (нереляційна БД) --");

        try
        {
            var database = MongoDbFactory.Create();

            await database.DropCollectionAsync("rooms");

            var service = new CrudServiceAsync<RoomDocument>(new MongoRepository<RoomDocument>(database, "rooms"));

            var room1 = new RoomDocument
            {
                RoomNumber = 101,
                RoomType = "стандарт",
                PricePerNight = 1200,
                HotelName = "Black Sea Resort",
                Amenities = new List<string> { "Wi-Fi" }
            };
            var room2 = new RoomDocument
            {
                RoomNumber = 102,
                RoomType = "стандарт",
                PricePerNight = 1350,
                HotelName = "Black Sea Resort",
                Amenities = new List<string> { "Wi-Fi", "Кондиціонер" }
            };
            var room3 = new RoomDocument
            {
                RoomNumber = 201,
                RoomType = "люкс",
                PricePerNight = 4200,
                HotelName = "Black Sea Resort",
                Amenities = new List<string> { "Wi-Fi", "Мінібар", "Кондиціонер" }
            };

            foreach (var room in new[] { room1, room2, room3 })
            {
                await service.CreateAsync(room);
            }
            Console.WriteLine("  Створено 3 документи у колекції rooms.");

            foreach (var room in await service.ReadAllAsync())
            {
                Console.WriteLine($"  №{room.RoomNumber} ({room.RoomType}): {room.PricePerNight:F2} грн, " +
                                  $"зручності: {string.Join(", ", room.Amenities)}");
            }

            var found = await service.ReadAsync(room2.Id);
            Console.WriteLine($"  ReadAsync: знайдено №{found.RoomNumber}");

            room2.PricePerNight = 1500;
            await service.UpdateAsync(room2);
            Console.WriteLine($"  UpdateAsync: нова ціна №{room2.RoomNumber} = {(await service.ReadAsync(room2.Id)).PricePerNight:F2} грн");

            var page = await service.ReadAllAsync(1, 2);
            Console.WriteLine($"  Пагінація (сторінка 1 по 2): {page.Count()} документи");

            await service.RemoveAsync(room1);
            Console.WriteLine($"  RemoveAsync: залишилось документів: {(await service.ReadAllAsync()).Count()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  MongoDB недоступна ({ex.GetType().Name}). Запустіть сервер MongoDB і повторіть. Демонстрацію пропущено.");
        }
    }
}