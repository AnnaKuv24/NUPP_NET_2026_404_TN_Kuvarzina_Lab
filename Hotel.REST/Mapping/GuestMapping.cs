using Hotel.Infrastructure.Models;
using Hotel.REST.Models;

namespace Hotel.REST.Mapping;

public static class GuestMapping
{
    public static GuestResponseModel ToResponse(this GuestModel e) => new()
    {
        Id = e.Id,
        FullName = e.FullName,
        PhoneNumber = e.PhoneNumber
    };

    public static GuestModel ToEntity(this GuestCreateModel m) => new()
    {
        Id = Guid.NewGuid(),
        FullName = m.FullName,
        PhoneNumber = m.PhoneNumber
    };

    public static void ApplyTo(this GuestUpdateModel m, GuestModel e)
    {
        e.FullName = m.FullName;
        e.PhoneNumber = m.PhoneNumber;
    }
}