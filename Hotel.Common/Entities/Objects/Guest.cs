namespace Hotel.Common.Entities.Objects;

public class Guest : IEntity
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string PassportNumber { get; set; }
    public List<Guid> BookedRoomIds { get; set; } = new();

    public Guest(string fullName, string passportNumber)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        PassportNumber = passportNumber;
    }

    public void BookRoom(Guid roomId)
    {
        BookedRoomIds.Add(roomId);
    }
}
