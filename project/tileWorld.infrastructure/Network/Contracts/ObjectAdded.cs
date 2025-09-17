using MemoryPack;

namespace tileWorld.infrastructure.Network.Contracts;
[MemoryPackable]
public partial class ObjectAdded
{
    public MapObjectDto Object { get; set; } = new();
}
