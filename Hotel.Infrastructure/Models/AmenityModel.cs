namespace Hotel.Infrastructure.Models;
public class AmenityModel : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<RoomModel> Rooms { get; set; } = new List<RoomModel>();
}