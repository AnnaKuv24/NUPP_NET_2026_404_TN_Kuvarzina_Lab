namespace Hotel.Infrastructure.Models;

///Гість. ОДИН-ДО-ОДНОГО з паспортом, ОДИН-ДО-БАГАТЬОХ з бронюваннями.
public class GuestModel : BaseModel
{
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    // ОДИН-ДО-ОДНОГО (головна сторона; залежна сторона — GuestPassportModel із зовнішнім ключем GuestId)
    public GuestPassportModel? Passport { get; set; }

    public ICollection<BookingModel> Bookings { get; set; } = new List<BookingModel>();
}