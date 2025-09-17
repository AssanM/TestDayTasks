using tileWorld.domain.Entities;
using tileWorld.domain.Events;

namespace tileWorld.domain.Interfaces;

public interface IObjectLayer
{
    Task<List<MapObject>> GetByAreaAsync(int x0, int y0, int x1, int y1);
    Task<MapObject?> GetByIdAsync(string id);
    Task AddAsync(MapObject obj);
    Task UpdateAsync(MapObject updated);
    Task DeleteAsync(string id);

    event EventHandler<MapObject> ObjectAdded;
    event EventHandler<ObjectUpdatedEventArgs> ObjectUpdated;
    event EventHandler<string> ObjectDeleted;

}
