namespace Hotel.REST.Models;

public class HotelResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int Stars { get; set; }
}