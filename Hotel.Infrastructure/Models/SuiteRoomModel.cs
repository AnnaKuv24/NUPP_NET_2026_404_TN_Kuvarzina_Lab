namespace Hotel.Infrastructure.Models;
public class SuiteRoomModel : RoomModel
{
    public int RoomsCount { get; set; }
    public bool HasJacuzzi { get; set; }
    public string ButlerServiceLevel { get; set; } = string.Empty;
}