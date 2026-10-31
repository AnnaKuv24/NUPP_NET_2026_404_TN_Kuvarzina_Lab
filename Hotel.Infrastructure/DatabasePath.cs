namespace Hotel.Infrastructure;
public static class DatabasePath
{
    public static string Resolve(string fileName = "hotel.db")
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && dir.GetFiles("*.sln").Length == 0)
        {
            dir = dir.Parent;
        }

        var root = dir?.FullName ?? AppContext.BaseDirectory;
        return Path.Combine(root, fileName);
    }
}