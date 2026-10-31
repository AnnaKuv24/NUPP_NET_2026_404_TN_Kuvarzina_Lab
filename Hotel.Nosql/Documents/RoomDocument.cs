using Hotel.Common.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Security.Principal;

namespace Hotel.Nosql.Documents;
public class RoomDocument : IEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();

    public int RoomNumber { get; set; }
    public string RoomType { get; set; } = string.Empty;
    public double PricePerNight { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public List<string> Amenities { get; set; } = new();
}