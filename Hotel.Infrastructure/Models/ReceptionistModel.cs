namespace Hotel.Infrastructure.Models;
public class ReceptionistModel : BaseModel
{
    public string FullName { get; set; } = string.Empty;
    public string ShiftTime { get; set; } = string.Empty;

    public Guid HotelId { get; set; }
    public HotelModel Hotel { get; set; } = null!;

    public ICollection<BookingModel> ConfirmedBookings { get; set; } = new List<BookingModel>();
}