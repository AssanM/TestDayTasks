using MemoryPack;

namespace tileWorld.infrastructure.Network.Contracts;
[MemoryPackable]
public partial class RegionDto
{
    public ushort Id { get; set; }
    public string Name { get; set; } = string.Empty;

}
