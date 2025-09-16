using testDay2.domain.Entities;

namespace testDay2.domain.Events
{
    public class ObjectUpdated
    {
        public MapObject OldObject { get; }
        public MapObject NewObject { get; }
        public ObjectUpdated(MapObject oldObject, MapObject newObject)
        {
            OldObject = oldObject;
            NewObject = newObject;
        }
    }
}
