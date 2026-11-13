using Hotel.Infrastructure.Models;
using Hotel.REST.Models;

namespace Hotel.REST.Mapping;

public static class HotelMapping
{
    public static HotelResponseModel ToResponse(this HotelModel e) => new()
    {
        Id = e.Id,
        Name = e.Name,
        City = e.City,
        Stars = e.Stars
    };

    public static HotelModel ToEntity(this HotelCreateModel m) => new()
    {
        Id = Guid.NewGuid(),
        Name = m.Name,
        City = m.City,
        Stars = m.Stars
    };

    public static void ApplyTo(this HotelUpdateModel m, HotelModel e)
    {
        e.Name = m.Name;
        e.City = m.City;
        e.Stars = m.Stars;
    }
}