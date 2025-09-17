using MemoryPack;

namespace tileWorld.infrastructure.Network.Contracts;
[MemoryPackable]
public partial class ObjectDeleted
{
    public string ObjectId { get; set; } = "";

}
