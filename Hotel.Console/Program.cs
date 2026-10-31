using Hotel.Common.Entities.Hierarchy;
using Hotel.Common.Entities.Objects;
using Hotel.Common.Services;
using Hotel.ConsoleApp.Demos;
using Hotel.Infrastructure;
using Hotel.Infrastructure.Mapping;
using Hotel.Infrastructure.Models;
using Hotel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

if (args.Contains("--legacy"))
{
    Lab1Demo.Run();
    await Lab2Demo.RunAsync();
    return;
}

Console.WriteLine("=== ЛР №3: Entity Framework Core + Repository + SQLite ===\n");

// true  -> при кожному запуску БД створюється з нуля 
// false -> дані накопичуються між запусками 
const bool resetDatabaseOnStart = true;

var dbPath = DatabasePath.Resolve();
var options = new DbContextOptionsBuilder<HotelContext>()
    .UseSqlite($"Data Source={dbPath}")
    .Options;

await using var context = new HotelContext(options);

// ---------------------------------------------------------------------
// 0. Підготовка БД: застосування міграцій
// ---------------------------------------------------------------------
if (!context.Database.GetMigrations().Any())
{
    throw new InvalidOperationException(
        "Міграцій не знайдено. Спочатку створіть міграцію: Add-Migration InitialCreate " +
        "(Default project = Hotel.Infrastructure, Startup project = Hotel.Console).");
}

if (resetDatabaseOnStart)
{
    await context.Database.EnsureDeletedAsync();
}

await context.Database.MigrateAsync();
Console.WriteLine($"База даних SQLite: {dbPath}");
Console.WriteLine("Застосовані міграції: " + string.Join(", ", await context.Database.GetAppliedMigrationsAsync()));

// ---------------------------------------------------------------------
// 1. CRUD-сервіси поверх репозиторіїв (один репозиторій на кожну сутність)
// ---------------------------------------------------------------------
var hotelService = new CrudServiceAsync<HotelModel>(new Repository<HotelModel>(context));
var amenityService = new CrudServiceAsync<AmenityModel>(new Repository<AmenityModel>(context));
var standardRoomService = new CrudServiceAsync<StandardRoomModel>(new Repository<StandardRoomModel>(context));
var suiteRoomService = new CrudServiceAsync<SuiteRoomModel>(new Repository<SuiteRoomModel>(context));
var guestService = new CrudServiceAsync<GuestModel>(new Repository<GuestModel>(context));
var receptionistService = new CrudServiceAsync<ReceptionistModel>(new Repository<ReceptionistModel>(context));
var bookingService = new CrudServiceAsync<BookingModel>(new Repository<BookingModel>(context));

// ---------------------------------------------------------------------
// 2. CREATE: наповнюємо БД. Номери/гості/адміністратор створюються як ДОМЕННІ об'єкти (ЛР1)
//    і перетворюються на моделі БД методом ToModel().
// ---------------------------------------------------------------------
Console.WriteLine("\n-- CREATE: наповнення бази даних --");

var hotel = new HotelModel { Name = "Black Sea Resort", City = "Одеса", Stars = 5 };
await hotelService.CreateAsync(hotel);

var wifi = new AmenityModel { Name = "Wi-Fi", Description = "Безкоштовний бездротовий інтернет" };
var minibar = new AmenityModel { Name = "Мінібар", Description = "Напої та закуски в номері" };
var airCon = new AmenityModel { Name = "Кондиціонер", Description = "Індивідуальне керування температурою" };
foreach (var amenity in new[] { wifi, minibar, airCon })
{
    await amenityService.CreateAsync(amenity);
}

var std1 = new StandardRoom(101, 1200m, 2, true, "у двір").ToModel(hotel.Id);
var std2 = new StandardRoom(102, 1350m, 2, true, "на море").ToModel(hotel.Id);
var std3 = new StandardRoom(103, 1100m, 1, false, "на вулицю").ToModel(hotel.Id);
var suite1 = new SuiteRoom(201, 4200m, 3, true, "преміум").ToModel(hotel.Id);

// зв'язок БАГАТО-ДО-БАГАТЬОХ
std1.Amenities.Add(wifi);
std2.Amenities.Add(wifi);
std2.Amenities.Add(airCon);
suite1.Amenities.Add(wifi);
suite1.Amenities.Add(minibar);
suite1.Amenities.Add(airCon);

foreach (var room in new[] { std1, std2, std3 })
{
    await standardRoomService.CreateAsync(room);
}
await suiteRoomService.CreateAsync(suite1);

//зв'язок ОДИН-ДО-ОДНОГО
var guest1 = new Guest("Олена Коваленко", "АА123456").ToModel("+380501112233", "МВС 1234", new DateTime(2032, 5, 20));
var guest2 = new Guest("Іван Петренко", "ВВ654321").ToModel("+380673334455", "МВС 5678", new DateTime(2030, 9, 1));
await guestService.CreateAsync(guest1);
await guestService.CreateAsync(guest2);

var receptionist = new Receptionist("Марія Шевченко", "08:00-20:00").ToModel(hotel.Id);
await receptionistService.CreateAsync(receptionist);

var booking1 = new BookingModel
{
    GuestId = guest1.Id,
    RoomId = std2.Id,
    ReceptionistId = receptionist.Id,
    CheckIn = new DateTime(2026, 11, 10),
    CheckOut = new DateTime(2026, 11, 13),
    TotalPrice = 3 * std2.PricePerNight
};
var booking2 = new BookingModel
{
    GuestId = guest1.Id,
    RoomId = suite1.Id,
    ReceptionistId = receptionist.Id,
    CheckIn = new DateTime(2026, 12, 20),
    CheckOut = new DateTime(2026, 12, 22),
    TotalPrice = 2 * suite1.PricePerNight
};
var booking3 = new BookingModel
{
    GuestId = guest2.Id,
    RoomId = std1.Id,
    ReceptionistId = receptionist.Id,
    CheckIn = new DateTime(2026, 11, 15),
    CheckOut = new DateTime(2026, 11, 16),
    TotalPrice = std1.PricePerNight
};
foreach (var booking in new[] { booking1, booking2, booking3 })
{
    await bookingService.CreateAsync(booking);
}

Console.WriteLine("Створено: 1 готель, 3 зручності, 3 стандартні номери, 1 люкс, 2 гостей (з паспортами), 1 адміністратора, 3 бронювання.");

// ---------------------------------------------------------------------
// 3. READ: ReadAllAsync, ReadAsync, пагінація
// ---------------------------------------------------------------------
Console.WriteLine("\n-- READ: усі стандартні номери (ReadAllAsync) --");
foreach (var room in await standardRoomService.ReadAllAsync())
{
    Console.WriteLine($"  №{room.RoomNumber}: {room.PricePerNight:F2} грн/ніч, ліжок: {room.BedsCount}, " +
                      $"Wi-Fi: {(room.HasWifi ? "так" : "ні")}, вид: {room.ViewType}");
}

Console.WriteLine("\n-- READ: один номер за Id (ReadAsync) --");
var foundRoom = await standardRoomService.ReadAsync(std2.Id);
Console.WriteLine($"  Знайдено номер №{foundRoom.RoomNumber}, вид: {foundRoom.ViewType}");

Console.WriteLine("\n-- ПАГІНАЦІЯ: сторінка 1 та 2 по 2 елементи --");
foreach (var pageNumber in new[] { 1, 2 })
{
    var pageItems = await standardRoomService.ReadAllAsync(pageNumber, 2);
    Console.WriteLine($"  Сторінка {pageNumber}: " + string.Join(", ", pageItems.Select(r => "№" + r.RoomNumber)));
}

// ---------------------------------------------------------------------
// 4. UPDATE: змінюємо ціну і перевіряємо ЧЕРЕЗ НОВИЙ контекст
// ---------------------------------------------------------------------
Console.WriteLine("\n-- UPDATE: підвищуємо ціну номера №102 до 1500 --");
std2.PricePerNight = 1500m;
await standardRoomService.UpdateAsync(std2);

await using (var verifyContext = new HotelContext(options))
{
    var fromDb = await new CrudServiceAsync<StandardRoomModel>(new Repository<StandardRoomModel>(verifyContext))
        .ReadAsync(std2.Id);
    Console.WriteLine($"  У базі (новий контекст): номер №{fromDb.RoomNumber}, ціна {fromDb.PricePerNight:F2} грн");
}

// ---------------------------------------------------------------------
// 5. DELETE: видаляємо номер без бронювань
// ---------------------------------------------------------------------
Console.WriteLine("\n-- DELETE: видаляємо номер №103 --");
await standardRoomService.RemoveAsync(std3);
Console.WriteLine($"  Залишилось стандартних номерів: {(await standardRoomService.ReadAllAsync()).Count()}");

// ---------------------------------------------------------------------
// 6. Зв'язки між таблицями 
// ---------------------------------------------------------------------
Console.WriteLine("\n-- ОДИН-ДО-БАГАТЬОХ: номери готелю --");
var hotelWithRooms = await context.Hotels.Include(h => h.Rooms).FirstAsync();
Console.WriteLine($"  Готель \"{hotelWithRooms.Name}\" ({hotelWithRooms.City}), номерів: {hotelWithRooms.Rooms.Count}");
foreach (var room in hotelWithRooms.Rooms.OrderBy(r => r.RoomNumber))
{
    Console.WriteLine($"    №{room.RoomNumber} — {(room is SuiteRoomModel ? "люкс" : "стандарт")}, {room.PricePerNight:F2} грн");
}

Console.WriteLine("\n-- ОДИН-ДО-ОДНОГО: гість і його паспорт --");
var guestsWithPassport = await context.Guests.Include(g => g.Passport).ToListAsync();
foreach (var guest in guestsWithPassport)
{
    Console.WriteLine($"  {guest.FullName}: паспорт {guest.Passport?.PassportNumber}, виданий {guest.Passport?.IssuedBy}");
}

Console.WriteLine("\n-- БАГАТО-ДО-БАГАТЬОХ: номери та їхні зручності --");
var roomsWithAmenities = await context.Rooms.Include(r => r.Amenities).OrderBy(r => r.RoomNumber).ToListAsync();
foreach (var room in roomsWithAmenities)
{
    Console.WriteLine($"  №{room.RoomNumber}: " + string.Join(", ", room.Amenities.Select(a => a.Name)));
}

Console.WriteLine("\n-- БАГАТО-ДО-БАГАТЬОХ (через Booking): хто який номер бронював --");
var bookings = await context.Bookings
    .Include(b => b.Guest)
    .Include(b => b.Room)
    .Include(b => b.Receptionist)
    .OrderBy(b => b.CheckIn)
    .ToListAsync();
foreach (var b in bookings)
{
    Console.WriteLine($"  {b.Guest.FullName} -> №{b.Room.RoomNumber}, {b.CheckIn:dd.MM.yyyy}-{b.CheckOut:dd.MM.yyyy}, " +
                      $"сума {b.TotalPrice:F2} грн (підтвердила: {b.Receptionist.FullName})");
}

// ---------------------------------------------------------------------
// 7. LINQ-статистика по даних із БД
// ---------------------------------------------------------------------
Console.WriteLine("\n-- LINQ: статистика цін усіх номерів --");
var allRooms = await context.Rooms.ToListAsync();   // рахуємо в пам'яті: SQLite слабко працює з decimal
Console.WriteLine($"  Мінімальна: {allRooms.Min(r => r.PricePerNight):F2} грн");
Console.WriteLine($"  Максимальна: {allRooms.Max(r => r.PricePerNight):F2} грн");
Console.WriteLine($"  Середня: {allRooms.Average(r => r.PricePerNight):F2} грн");

// ---------------------------------------------------------------------
// 8. Додаткове завдання
// ---------------------------------------------------------------------
await NosqlDemo.RunAsync();

Console.WriteLine("\n=== Кінець демонстрації ЛР №3 ===");