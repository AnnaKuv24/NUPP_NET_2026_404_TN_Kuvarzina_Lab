namespace Hotel.Common.Entities.Hierarchy;

public class StandardRoom : Room
{
    public int BedsCount { get; set; }
    public bool HasWifi { get; set; }
    public string ViewType { get; set; }

    public StandardRoom(int roomNumber, decimal pricePerNight, int bedsCount, bool hasWifi, string viewType)
        : base(roomNumber, pricePerNight)
    {
        BedsCount = bedsCount;
        HasWifi = hasWifi;
        ViewType = viewType;
    }

    public override string GetInfo()
    {
        return base.GetInfo() + $" — стандартний номер, ліжок: {BedsCount}, Wi-Fi: {(HasWifi ? "так" : "ні")}, вид: {ViewType}";
    }
}
