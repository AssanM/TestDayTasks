using tileWorld.domain.ValueObjects;

namespace tileWorld.application.Models;

public class FillRequest
{
    public int XStart { get; set; }
    public int YStart { get; set; }
    public int XEnd { get; set; }
    public int YEnd { get; set; }
    public SurfaceType Type { get; set; }
}
