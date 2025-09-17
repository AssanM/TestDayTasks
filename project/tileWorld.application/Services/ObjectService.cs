using tileWorld.domain.Entities;
using tileWorld.domain.Events;
using tileWorld.domain.Interfaces;

namespace tileWorld.application.Services;

public class ObjectService : IObjectLayer
{
    private readonly List<MapObject> _objects = new();

    public event EventHandler<MapObject>? ObjectAdded;
    public event EventHandler<ObjectUpdatedEventArgs>? ObjectUpdated;
    public event EventHandler<string>? ObjectDeleted;

    public Task<List<MapObject>> GetByAreaAsync(int x0, int y0, int x1, int y1) =>
        Task.FromResult(_objects.Where(o => o.Intersects(x0, y0, x1, y1)).ToList());

    public Task<MapObject?> GetByIdAsync(string id) =>
        Task.FromResult(_objects.FirstOrDefault(o => o.Id == id));

    public Task AddAsync(MapObject obj)
    {
        _objects.Add(obj);
        ObjectAdded?.Invoke(this, obj);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(MapObject updated)
    {
        var index = _objects.FindIndex(o => o.Id == updated.Id);
        if (index >= 0)
        {
            var old = _objects[index];
            _objects[index] = updated;
            ObjectUpdated?.Invoke(this, new ObjectUpdatedEventArgs(old, updated));
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string id)
    {
        var obj = _objects.FirstOrDefault(o => o.Id == id);
        if (obj != null)
        {
            _objects.Remove(obj);
            ObjectDeleted?.Invoke(this, id);
        }
        return Task.CompletedTask;
    }

}
