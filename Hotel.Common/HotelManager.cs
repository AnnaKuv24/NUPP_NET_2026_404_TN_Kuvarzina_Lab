using Hotel.Common.Entities.Objects;
using Hotel.Common.Services;

namespace Hotel.Common;

public class HotelManager
{
    public CrudService<Guest> Guests { get; } = new();

    private static int _instancesCreated;

    static HotelManager()
    {
        _instancesCreated = 0;
    }

    public HotelManager()
    {
        _instancesCreated++;
    }

    public void RegisterReceptionist(Receptionist receptionist)
    {
        // підписуємось на подію конкретного адміністратора
        receptionist.BookingConfirmed += OnBookingConfirmed;
    }

    // обробник події
    private void OnBookingConfirmed(Receptionist receptionist, Guid roomId, string roomInfo)
    {
        Console.WriteLine($">> Подія: адміністратор \"{receptionist.FullName}\" підтвердив бронювання \"{roomInfo}\" (Id={roomId}).");
    }

    public static HotelManager CreateDefault()
    {
        Console.WriteLine($"Створюємо HotelManager. Всього створено екземплярів: {_instancesCreated + 1}.");
        return new HotelManager();
    }
}
