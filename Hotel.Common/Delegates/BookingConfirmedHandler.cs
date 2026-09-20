using Hotel.Common.Entities.Objects;

namespace Hotel.Common.Delegates;

public delegate void BookingConfirmedHandler(Receptionist receptionist, Guid roomId, string roomInfo);
