using tileWorld.domain.Entities;

namespace tileWorld.domain.Events;

public class ObjectUpdatedEventArgs : EventArgs
{
    public MapObject OldObject { get; }
    public MapObject NewObject { get; }

    public ObjectUpdatedEventArgs(MapObject oldObject, MapObject newObject)
    {
        OldObject = oldObject;
        NewObject = newObject;
    }

}
