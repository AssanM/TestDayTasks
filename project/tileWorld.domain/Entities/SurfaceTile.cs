using tileWorld.domain.ValueObjects;

namespace tileWorld.domain.Entities;

public class SurfaceTile
{
    public SurfaceType Type { get; set; }

    public SurfaceTile(SurfaceType type)
    {
        Type = type;
    }
    public bool CanPlaceObject => Type == SurfaceType.Plain;
    public void SetType(SurfaceType type) => Type = type;
}
