namespace Hotel.Common.Entities.Hierarchy;

public abstract class Room : IEntity
{
    public Guid Id { get; set; }
    public int RoomNumber { get; set; }
    public decimal PricePerNight { get; set; }

    public static int TotalRoomsCount { get; private set; }

    // Викликається один раз, автоматично, перед першим використанням класу.
    static Room()
    {
        TotalRoomsCount = 0;
        Console.WriteLine("[Room] Статичний конструктор викликано.");
    }

    protected Room(int roomNumber, decimal pricePerNight)
    {
        Id = Guid.NewGuid();
        RoomNumber = roomNumber;
        PricePerNight = pricePerNight;
        TotalRoomsCount++;
    }

    public virtual string GetInfo()
    {
        return $"[{Id}] Номер №{RoomNumber}, ціна за ніч: {PricePerNight:F2} грн";
    }
}
