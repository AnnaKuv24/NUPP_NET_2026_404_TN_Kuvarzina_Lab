namespace Hotel.Infrastructure.Models;
public class GuestPassportModel : BaseModel
{
    public string PassportNumber { get; set; } = string.Empty;
    public string IssuedBy { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }

    public Guid GuestId { get; set; }
    public GuestModel Guest { get; set; } = null!;
}