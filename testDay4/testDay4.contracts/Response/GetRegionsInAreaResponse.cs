using MemoryPack;

namespace testDay4.contracts.Response;

public class GetRegionsInAreaResponse
{
    public List<RegionDto> Regions { get; set; } = new();

}
[MemoryPackable]
public partial class RegionDto
{
    public ushort Id { get; set; }
    public string Name { get; set; } = string.Empty;

}
