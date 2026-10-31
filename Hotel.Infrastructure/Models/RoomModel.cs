namespace Hotel.Infrastructure.Models;
public abstract class RoomModel : BaseModel
{
    public int RoomNumber { get; set; }
    public decimal PricePerNight { get; set; }

    // ОДИН-ДО-БАГАТЬОХ: багато номерів -> один готель
    public Guid HotelId { get; set; }
    public HotelModel Hotel { get; set; } = null!;

    // ОДИН-ДО-БАГАТЬОХ: один номер -> багато бронювань
    public ICollection<BookingModel> Bookings { get; set; } = new List<BookingModel>();

    // БАГАТО-ДО-БАГАТЬОХ: номер має багато зручностей, зручність є у багатьох номерах
    public ICollection<AmenityModel> Amenities { get; set; } = new List<AmenityModel>();
}