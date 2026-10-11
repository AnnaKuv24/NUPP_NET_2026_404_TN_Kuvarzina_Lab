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

    // конструкція: статичний фабричний метод, що генерує об'єкт із випадковими даними.
    private static readonly string[] Views = { "у двір", "на море", "на гори", "на вулицю" };

    public static StandardRoom CreateNew()
    {
        var number = Random.Shared.Next(100, 999);
        var price = Random.Shared.Next(800, 2000);
        var beds = Random.Shared.Next(1, 4);
        var hasWifi = Random.Shared.Next(0, 2) == 1;
        var view = Views[Random.Shared.Next(Views.Length)];

        return new StandardRoom(number, price, beds, hasWifi, view);
    }
}
