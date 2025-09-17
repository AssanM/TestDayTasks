using tileWorld.domain.Entities;
using tileWorld.domain.Interfaces;
using tileWorld.domain.ValueObjects;

namespace tileWorld.application.Services;

public class SurfaceLayer : ISurfaceLayer
{
    private readonly SurfaceTile[,] _tiles;
    public int Width { get; }
    public int Height { get; }
    public SurfaceLayer(int width, int height)
    {
        Width = width;
        Height = height;
        _tiles = new SurfaceTile[Width, Height];
        for (int i = 0; i < height; i++)
            for (int j = 0; j < Width; j++)
                _tiles[i, j] = new SurfaceTile(SurfaceType.Plain);
    }
    public async Task<SurfaceType> GetTileAsync(int x, int y)
    {
        if (!IsInBounds(x, y))
            throw new ArgumentOutOfRangeException();
        return await Task.FromResult(_tiles[x, y].Type);
    }
    public async Task SetTileAsync(int x, int y, SurfaceType type)
    {
        if (!IsInBounds(x, y))
            throw new ArgumentOutOfRangeException();
        await Task.Run(() => _tiles[x, y].SetType(type));
    }
    public bool IsInBounds(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;
    public async Task FillArea(int xStart, int yStart, int xEnd, int yEnd, SurfaceType type)
    {
        await Task.Run(() =>
        {
            for (int i = yStart; i <= yEnd; i++)
                for (int j = xStart; j <= xEnd; j++)
                    _tiles[i, j] = new SurfaceTile(SurfaceType.Plain);
        });

    }

    public async Task<bool> CanPlaceObjectInArea(int xStart, int yStart, int xEnd, int yEnd)
    {
        return await Task.Run(() =>
        {
            for (int i = yStart; i <= yEnd; i++)
                for (int j = xStart; j <= xEnd; j++)
                    if (!IsInBounds(j, i) || !_tiles[j, i].CanPlaceObject)
                        return false;
            return true;
        });
    }
}
