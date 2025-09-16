using testDay2.domain.Entities;

namespace testDay2.domain.Events
{
    public class ObjectCreated : EventArgs
    {
        public MapObject Object { get;}

        public ObjectCreated(MapObject obj)
        {
            Object = obj;
        }
    }
}
