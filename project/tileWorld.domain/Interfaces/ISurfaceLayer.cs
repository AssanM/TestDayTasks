using tileWorld.domain.ValueObjects;

namespace tileWorld.domain.Interfaces;

public interface ISurfaceLayer
{
    int Width { get; }
    int Height { get; }
    Task<SurfaceType> GetTileAsync(int x, int y);
    bool IsInBounds(int x, int y);
    Task FillArea(int xStart, int yStart, int xEnd, int yEnd, SurfaceType type);
    Task<bool> CanPlaceObjectInArea(int xStart, int yStart, int xEnd, int yEnd);
    Task SetTileAsync(int x, int y, SurfaceType type);
}

