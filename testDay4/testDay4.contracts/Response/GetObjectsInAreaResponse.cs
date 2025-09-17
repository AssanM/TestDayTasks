using MemoryPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testDay4.contracts.Response;
[MemoryPackable]
public partial class GetObjectsInAreaResponse
{
    public List<MapObjectDto> Objects { get; set; }
}

[MemoryPackable]
public partial class MapObjectDto
{
    public string Id { get; set; } = string.Empty;
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}
