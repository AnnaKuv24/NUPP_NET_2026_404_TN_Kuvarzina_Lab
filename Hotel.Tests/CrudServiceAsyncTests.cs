using Hotel.Common.Entities.Hierarchy;
using Hotel.Common.Services;
using Xunit;

namespace Hotel.Tests;

public class CrudServiceAsyncTests
{
    private static CrudServiceAsync<StandardRoom> CreateService()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"test_rooms_{Guid.NewGuid()}.json");
        return new CrudServiceAsync<StandardRoom>(tempFile);
    }

    [Fact]
    public async Task CreateAsync_AddsElement_ReadAsyncReturnsIt()
    {
        var service = CreateService();
        var room = new StandardRoom(101, 1000m, 2, true, "у двір");

        var created = await service.CreateAsync(room);
        var fetched = await service.ReadAsync(room.Id);

        Assert.True(created);
        Assert.Equal(room.Id, fetched.Id);
    }

    [Fact]
    public async Task ReadAsync_UnknownId_ThrowsKeyNotFoundException()
    {
        var service = CreateService();

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.ReadAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task UpdateAsync_ExistingElement_UpdatesValue()
    {
        var service = CreateService();
        var room = new StandardRoom(101, 1000m, 2, true, "у двір");
        await service.CreateAsync(room);

        room.PricePerNight = 1500m;
        var updated = await service.UpdateAsync(room);
        var fetched = await service.ReadAsync(room.Id);

        Assert.True(updated);
        Assert.Equal(1500m, fetched.PricePerNight);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingElement_ReturnsFalse()
    {
        var service = CreateService();
        var room = new StandardRoom(101, 1000m, 2, true, "у двір"); // ніколи не створювався

        var updated = await service.UpdateAsync(room);

        Assert.False(updated);
    }

    [Fact]
    public async Task RemoveAsync_ExistingElement_RemovesIt()
    {
        var service = CreateService();
        var room = new StandardRoom(101, 1000m, 2, true, "у двір");
        await service.CreateAsync(room);

        var removed = await service.RemoveAsync(room);
        var all = await service.ReadAllAsync();

        Assert.True(removed);
        Assert.Empty(all);
    }

    [Fact]
    public async Task ReadAllAsync_WithPagination_ReturnsCorrectSlice()
    {
        var service = CreateService();
        for (int i = 0; i < 10; i++)
            await service.CreateAsync(StandardRoom.CreateNew());

        var page1 = (await service.ReadAllAsync(page: 1, amount: 4)).ToList();
        var page2 = (await service.ReadAllAsync(page: 2, amount: 4)).ToList();
        var page3 = (await service.ReadAllAsync(page: 3, amount: 4)).ToList();

        Assert.Equal(4, page1.Count);
        Assert.Equal(4, page2.Count);
        Assert.Equal(2, page3.Count); // залишок: 10 - 4 - 4 = 2
    }

    [Fact]
    public async Task CreateAsync_CalledInParallel_AllElementsSaved()
    {
        var service = CreateService();

        // паралельно створюємо 500 елементів — перевіряємо thread-safety колекції
        await Parallel.ForEachAsync(Enumerable.Range(0, 500), async (_, _) =>
        {
            await service.CreateAsync(StandardRoom.CreateNew());
        });

        var all = await service.ReadAllAsync();

        Assert.Equal(500, all.Count());
    }

    [Fact]
    public async Task SaveAsync_Then_LoadAsync_RestoresData()
    {
        var service = CreateService();
        var room = new StandardRoom(101, 1000m, 2, true, "у двір");
        await service.CreateAsync(room);

        var saved = await service.SaveAsync();
        var restoredService = new CrudServiceAsync<StandardRoom>(
            GetPrivateFilePath(service));
        var loaded = await restoredService.LoadAsync();
        var restored = await restoredService.ReadAllAsync();

        Assert.True(saved);
        Assert.True(loaded);
        Assert.Single(restored);
    }

    private static string GetPrivateFilePath(CrudServiceAsync<StandardRoom> service)
    {
        var field = typeof(CrudServiceAsync<StandardRoom>)
            .GetField("_filePath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
        return (string)field.GetValue(service)!;
    }
}