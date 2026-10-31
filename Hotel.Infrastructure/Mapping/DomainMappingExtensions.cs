using Hotel.Common.Entities.Hierarchy;
using Hotel.Common.Entities.Objects;
using Hotel.Infrastructure.Models;

namespace Hotel.Infrastructure.Mapping;
public static class DomainMappingExtensions
{
    public static StandardRoomModel ToModel(this StandardRoom room, Guid hotelId)
    {
        return new StandardRoomModel
        {
            Id = room.Id,
            RoomNumber = room.RoomNumber,
            PricePerNight = room.PricePerNight,
            HotelId = hotelId,
            BedsCount = room.BedsCount,
            HasWifi = room.HasWifi,
            ViewType = room.ViewType
        };
    }

    public static SuiteRoomModel ToModel(this SuiteRoom room, Guid hotelId)
    {
        return new SuiteRoomModel
        {
            Id = room.Id,
            RoomNumber = room.RoomNumber,
            PricePerNight = room.PricePerNight,
            HotelId = hotelId,
            RoomsCount = room.RoomsCount,
            HasJacuzzi = room.HasJacuzzi,
            ButlerServiceLevel = room.ButlerServiceLevel
        };
    }

    public static GuestModel ToModel(this Guest guest, string phoneNumber, string passportIssuedBy, DateTime passportExpiry)
    {
        return new GuestModel
        {
            Id = guest.Id,
            FullName = guest.FullName,
            PhoneNumber = phoneNumber,
            Passport = new GuestPassportModel
            {
                PassportNumber = guest.PassportNumber,
                IssuedBy = passportIssuedBy,
                ExpiryDate = passportExpiry
            }
        };
    }

    public static ReceptionistModel ToModel(this Receptionist receptionist, Guid hotelId)
    {
        return new ReceptionistModel
        {
            Id = receptionist.Id,
            FullName = receptionist.FullName,
            ShiftTime = receptionist.ShiftTime,
            HotelId = hotelId
        };
    }
}