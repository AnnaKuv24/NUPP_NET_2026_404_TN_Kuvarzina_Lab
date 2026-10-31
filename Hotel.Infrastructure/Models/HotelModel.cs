namespace Hotel.Infrastructure.Models;
public class HotelModel : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int Stars { get; set; }

    public ICollection<RoomModel> Rooms { get; set; } = new List<RoomModel>();
    public ICollection<ReceptionistModel> Receptionists { get; set; } = new List<ReceptionistModel>();
}