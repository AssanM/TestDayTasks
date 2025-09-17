using testDay4.domain.Entities;
using testDay4.domain.Interfaces;
namespace testDay4.application.Services;

public class ObjectService : IObjectLayer
{
    private readonly List<MapObject> _objects = new();
    public event EventHandler<MapObject>? ObjectAdded;
    public event EventHandler<(MapObject, MapObject)>? ObjectUpdated;
    public event EventHandler<string>? ObjectDeleted;

    public Task<List<MapObject>> GetByAreaAsync(int x0, int y0, int x1, int y1)
    {
        var result = _objects.Where(o => o.Intersects(x0, y0, x1, y1)).ToList();
        return Task.FromResult(result);
    }

    public void Add(MapObject obj)
    {
        _objects.Add(obj);
        ObjectAdded?.Invoke(this, obj);
    }

    public void Update(MapObject updated)
    {
        var index = _objects.FindIndex(o => o.Id == updated.Id);
        if (index >= 0)
        {
            var old = _objects[index];
            _objects[index] = updated;
            ObjectUpdated?.Invoke(this, (old, updated));
        }
    }

    public void Delete(string id)
    {
        var obj = _objects.FirstOrDefault(o => o.Id == id);
        if (obj != null)
        {
            _objects.Remove(obj);
            ObjectDeleted?.Invoke(this, id);
        }
    }

}
