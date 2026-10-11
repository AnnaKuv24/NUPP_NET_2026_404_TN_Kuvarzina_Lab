namespace Hotel.Common.Entities.Hierarchy;

public class SuiteRoom : Room
{
    public int RoomsCount { get; set; }
    public bool HasJacuzzi { get; set; }
    public string ButlerServiceLevel { get; set; }

    public SuiteRoom(int roomNumber, decimal pricePerNight, int roomsCount, bool hasJacuzzi, string butlerServiceLevel)
        : base(roomNumber, pricePerNight)
    {
        RoomsCount = roomsCount;
        HasJacuzzi = hasJacuzzi;
        ButlerServiceLevel = butlerServiceLevel;
    }

    // перевизначений метод батьківського класу
    public override string GetInfo()
    {
        return base.GetInfo() + $" — люкс, кімнат: {RoomsCount}, джакузі: {(HasJacuzzi ? "так" : "ні")}, рівень обслуговування: {ButlerServiceLevel}";
    }

    // конструкція: статичний фабричний метод, що генерує об'єкт із випадковими даними.
    private static readonly string[] ServiceLevels = { "базовий", "преміум", "VIP" };

    public static SuiteRoom CreateNew()
    {
        var number = Random.Shared.Next(1000, 1999);
        var price = Random.Shared.Next(3000, 8000);
        var rooms = Random.Shared.Next(2, 5);
        var hasJacuzzi = Random.Shared.Next(0, 2) == 1;
        var level = ServiceLevels[Random.Shared.Next(ServiceLevels.Length)];

        return new SuiteRoom(number, price, rooms, hasJacuzzi, level);
    }
}
