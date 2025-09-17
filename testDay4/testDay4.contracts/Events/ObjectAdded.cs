using MemoryPack;
using testDay4.contracts.Response;

namespace testDay4.contracts.Events;

[MemoryPackable]
public partial class ObjectAdded
{
    public MapObjectDto Object { get; set; } = new();
}
