using Hotel.Common.Delegates;

namespace Hotel.Common.Entities.Objects;

public class Receptionist : IEntity
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string ShiftTime { get; set; }

    public event BookingConfirmedHandler? BookingConfirmed;

    public Receptionist(string fullName, string shiftTime)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        ShiftTime = shiftTime;
    }

    public void ConfirmBooking(Guid roomId, string roomInfo)
    {
        // виклик події — повідомляємо усіх підписників
        BookingConfirmed?.Invoke(this, roomId, roomInfo);
    }

    public static Receptionist CreateSystemReceptionist()
    {
        return new Receptionist("Черговий адміністратор", "цілодобово");
    }
}
