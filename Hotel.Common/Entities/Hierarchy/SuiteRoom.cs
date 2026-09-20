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
}
