namespace Hotel.Infrastructure.Models;

/// Бронювання — "зв'язувальна" сутність: фактично реалізує БАГАТО-ДО-БАГАТЬОХ між Guest та Room 

public class BookingModel : BaseModel
{
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public decimal TotalPrice { get; set; }

    public Guid GuestId { get; set; }
    public GuestModel Guest { get; set; } = null!;

    public Guid RoomId { get; set; }
    public RoomModel Room { get; set; } = null!;

    public Guid ReceptionistId { get; set; }
    public ReceptionistModel Receptionist { get; set; } = null!;
}