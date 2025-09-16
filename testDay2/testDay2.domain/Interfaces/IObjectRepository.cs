using testDay2.domain.Entities;
using testDay2.domain.Events;

namespace testDay2.domain.Interfaces;

public interface IObjectRepository
{
    Task AddAsync(MapObject obj);
    Task<MapObject?> GetByIdAsync(string id);
    Task RemoveAsync(string id);
    Task<List<MapObject>> GetByPointAsync(int x, int y);
    Task<List<MapObject>> GetByAreaAsync(int x0, int y0, int x1, int y1);
    event EventHandler<ObjectCreated>? ObjectCreated;
    event EventHandler<ObjectUpdated>? ObjectUpdated;
    Task UpdateAsync(MapObject updatedObject);
    event EventHandler<string>? ObjectDeleted;

}
