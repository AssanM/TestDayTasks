using MemoryPack;

namespace tileWorld.infrastructure.Network.Contracts;
[MemoryPackable]
public class GetRegionsInAreaResponse
{
    public List<RegionDto> Regions { get; set; } = new();
}
