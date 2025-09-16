using testDay3.domain.Entities;
using testDay3.domain.Interfaces;

namespace testDay3.application.Services;

public class RegionLayer : IRegionLayer
{
    private ushort[,] _regionMap = new ushort[0,0];
    private readonly Dictionary<ushort, Region> _regions = new();

    public async Task GenerateRegionsAsync(int mapWidth, int mapHeight, int regionCount)
    {
        await Task.Run(() =>
        {
            _regionMap = new ushort[mapWidth, mapHeight];
            int tilesPerRegion = (mapWidth * mapHeight) / regionCount;
            int regionWidth = (int)Math.Sqrt(tilesPerRegion);
            int regionHeight = regionWidth;
            ushort id = 1;
            for (int y=0;y<mapHeight;y+=regionHeight)
                for (int x=0;x<mapWidth;x+=regionWidth)
                {
                    var name = $"Region #{id}";
                    _regions[id] = new Region(id, name);
                    for (int dy = 0; dy < regionHeight && y + dy < mapHeight; dy++)
                        for (int dx = 0; dx < regionWidth && x + dx < mapWidth; dx++)
                            _regionMap[x + dx, y + dy] = id;
                    id++;
                    if (id > regionCount) return;
                }
        });
    }

    public Task<ushort> GetRegionIdAtAsync(int x, int y) => Task.FromResult(_regionMap[x, y]);

    public Task<Region> GetRegionByIdAsync(ushort id)=>Task.FromResult(_regions[id]);

    public Task<bool> TileBelongsToRegionAsync(int x, int y, ushort regionId) => Task.FromResult(_regionMap[x, y] == regionId);

    public async Task<List<Region>> GetRegionsInAreaAsync(int x0, int y0, int x1, int y1)
    {
        return await Task.Run(() =>
        {
            var found = new HashSet<ushort>();
            for (int y = y0; y <= y1; y++)
                for (int x = x0; x <= x1; x++)
                    found.Add(_regionMap[x, y]);
            return found.Select(id => _regions[id]).ToList();
        });
    }
}
