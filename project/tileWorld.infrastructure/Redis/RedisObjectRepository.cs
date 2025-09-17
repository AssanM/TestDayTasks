using StackExchange.Redis;
using System.Text.Json;
using tileWorld.domain.Entities;
using tileWorld.domain.Events;
using tileWorld.domain.Interfaces;
using tileWorld.infrastructure.Network.Contracts;

namespace tileWorld.infrastructure.Redis;

public class RedisObjectRepository : IObjectRepository
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _database;
    private const string GeoKey = "map_objects";

    public event EventHandler<ObjectCreated>? ObjectCreated;
    public event EventHandler<ObjectUpdatedEventArgs>? ObjectUpdated;
    public event EventHandler<string>? ObjectDeleted;

    public RedisObjectRepository(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _database = _redis.GetDatabase();
    }

    public async Task AddAsync(MapObject obj)
    {
        var (lon, lat) = ToGeo(obj.X, obj.Y);
        await _database.GeoAddAsync(GeoKey, new GeoEntry(lon, lat, obj.Id));
        await _database.StringSetAsync($"obj:{obj.Id}", JsonSerializer.Serialize(obj));
        ObjectCreated?.Invoke(this, new ObjectCreated(obj));
    }

    private (double lon, double lat) ToGeo(int x, int y)
    {
        return (x * 0.0001, y * 0.0001);
    }

    public async Task<MapObject> GetByIdAsync(string id)
    {
        var json = await _database.StringGetAsync($"obj:{id}");
        return json.HasValue ? JsonSerializer.Deserialize<MapObject>(json!) : null;
    }

    public async Task RemoveAsync(string id)
    {
        await _database.KeyDeleteAsync($"obj:{id}");
        await _database.GeoRemoveAsync(GeoKey, id);
        ObjectDeleted?.Invoke(this, id);
    }

    public async Task<List<MapObject>> GetByPointAsync(int x, int y)
    {
        var (lon, lat) = ToGeo(x, y);
        var results = await _database.GeoRadiusAsync(GeoKey, lon, lat, 1, GeoUnit.Kilometers);
        var list = new List<MapObject>();

        foreach (var entry in results)
        {
            var obj = await GetByIdAsync(entry.Member!);
            if (obj != null && obj.Contains(x, y))
                list.Add(obj);
        }

        return list;
    }

    public async Task<List<MapObject>> GetByAreaAsync(int x0, int y0, int x1, int y1)
    {
        var centerX = (x0 + x1) / 2;
        var centerY = (y0 + y1) / 2;
        var radius = Math.Max(x1 - x0, y1 - y0) / 2;

        var (lon, lat) = ToGeo(centerX, centerY);
        var results = await _database.GeoRadiusAsync(GeoKey, lon, lat, radius, GeoUnit.Kilometers);
        var list = new List<MapObject>();

        foreach (var entry in results)
        {
            var obj = await GetByIdAsync(entry.Member!);
            if (obj != null && obj.Intersects(x0, y0, x1, y1))
                list.Add(obj);
        }

        return list;
    }

    public async Task UpdateAsync(MapObject updatedObject)
    {
        var existing = await GetByIdAsync(updatedObject.Id);
        if (existing == null) throw new InvalidOperationException("Object not found");
        await _database.GeoRemoveAsync(GeoKey, updatedObject.Id);
        var (lon, lat) = ToGeo(updatedObject.X, updatedObject.Y);
        await _database.GeoAddAsync(GeoKey, new GeoEntry(lon, lat, updatedObject.Id));
        await _database.StringSetAsync($"obj:{updatedObject.Id}", JsonSerializer.Serialize(updatedObject));

        ObjectUpdated?.Invoke(this, new ObjectUpdatedEventArgs(existing, updatedObject));
    }
}
