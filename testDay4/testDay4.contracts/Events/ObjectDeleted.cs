using MemoryPack;

namespace testDay4.contracts.Events;
[MemoryPackable]
public class ObjectDeleted
{
    public string ObjectId { get; set; }
}
