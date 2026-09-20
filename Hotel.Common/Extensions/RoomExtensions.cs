using Hotel.Common.Entities.Hierarchy;

namespace Hotel.Common.Extensions;

public static class RoomExtensions
{
    public static string ToShortString(this Room room)
    {
        return $"Номер №{room.RoomNumber} — {room.PricePerNight:F2} грн/ніч";
    }
}
