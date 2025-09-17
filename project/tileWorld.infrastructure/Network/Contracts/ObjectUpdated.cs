using MemoryPack;

namespace tileWorld.infrastructure.Network.Contracts;
[MemoryPackable]
public partial class ObjectUpdated
{
    public MapObjectDto Object { get; set; } = new();

}
