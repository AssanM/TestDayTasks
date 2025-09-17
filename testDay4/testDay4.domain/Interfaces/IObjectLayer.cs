using testDay4.domain.Entities;

namespace testDay4.domain.Interfaces;

public interface IObjectLayer
{
    Task<List<MapObject>> GetByAreaAsync(int x0, int y0, int x1, int y1);
    event EventHandler<MapObject> ObjectAdded;
    event EventHandler<(MapObject OldObject, MapObject NewObject)> ObjectUpdated;
    event EventHandler<string> ObjectDeleted;

}
