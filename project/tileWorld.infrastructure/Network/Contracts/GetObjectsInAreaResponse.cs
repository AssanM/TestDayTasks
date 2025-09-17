using MemoryPack;

namespace tileWorld.infrastructure.Network.Contracts;
[MemoryPackable]
public partial class GetObjectsInAreaResponse
{
    public List<MapObjectDto> Objects { get; set; }
}
