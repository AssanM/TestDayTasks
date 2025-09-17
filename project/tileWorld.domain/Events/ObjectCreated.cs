using tileWorld.domain.Entities;

namespace tileWorld.domain.Events;

public class ObjectCreated : EventArgs
{
    public MapObject Object { get; }

    public ObjectCreated(MapObject obj)
    {
        Object = obj;
    }
}
