using MemoryPack;
using testDay4.contracts.Response;

namespace testDay4.contracts.Events;
[MemoryPackable]
public class ObjectUpdated
{
    public MapObjectDto OldObject { get; }
    public MapObjectDto NewObject { get; set; }
    
}
