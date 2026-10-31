using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Hotel.Infrastructure;
public class HotelContextFactory : IDesignTimeDbContextFactory<HotelContext>
{
    public HotelContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<HotelContext>()
            .UseSqlite($"Data Source={DatabasePath.Resolve()}")
            .Options;

        return new HotelContext(options);
    }
}