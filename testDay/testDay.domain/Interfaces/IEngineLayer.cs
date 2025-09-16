using testDay.domain.ValueObjects;

namespace testDay.domain.Interfaces;

public interface IEngineLayer
{
    int Width { get; }
    int Height { get; }
    Task<EngineType> GetTileAsync(int x, int y);
    bool IsInBounds(int x, int y);  
    Task FillArea(int xStart, int yStart, int xEnd, int yEnd, EngineType type);
    Task<bool> CanPlaceObjectInArea(int xStart, int yStart, int xEnd, int yEnd);
    Task SetTileAsync(int x, int y, EngineType type);
}
