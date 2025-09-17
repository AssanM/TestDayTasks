using tileWorld.domain.Entities;

namespace tileWorld.domain.Interfaces;

public interface IRegionLayer
{
    Task GenerateRegionsAsync(int mapWidth, int mapHeight, int regionCount);
    Task<ushort> GetRegionIdAtAsync(int x, int y);
    Task<Region> GetRegionByIdAsync(ushort id);
    Task<bool> TileBelongsToRegionAsync(int x, int y, ushort regionId);
    Task<List<Region>> GetRegionsInAreaAsync(int x0, int y0, int x1, int y1);

}
