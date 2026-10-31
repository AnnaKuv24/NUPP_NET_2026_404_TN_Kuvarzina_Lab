namespace Hotel.Infrastructure.Models;
public class StandardRoomModel : RoomModel
{
    public int BedsCount { get; set; }
    public bool HasWifi { get; set; }
    public string ViewType { get; set; } = string.Empty;
}