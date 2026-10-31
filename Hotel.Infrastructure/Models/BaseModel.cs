using Hotel.Common.Entities;

namespace Hotel.Infrastructure.Models;
public abstract class BaseModel : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
}