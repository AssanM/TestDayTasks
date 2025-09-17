using tileWorld.application.Services;

namespace tileWorld.tests;

public class RegionLayerTests
{
    [Fact]
    public async Task GenerateRegions_AssignsAllTiles()
    {
        var layer = new RegionLayer();
        await layer.GenerateRegionsAsync(100, 100, 4);

        for (int y = 0; y < 100; y++)
            for (int x = 0; x < 100; x++)
                Assert.True(await layer.GetRegionIdAtAsync(x, y) > 0);
    }

    [Fact]
    public async Task GetRegionById_ReturnsCorrectName()
    {
        var layer = new RegionLayer();
        await layer.GenerateRegionsAsync(50, 50, 2);
        var region = await layer.GetRegionByIdAsync(1);
        Assert.StartsWith("Регион #", region.Name);
    }

    [Fact]
    public async Task TileBelongsToRegion_Works()
    {
        var layer = new RegionLayer();
        await layer.GenerateRegionsAsync(20, 20, 1);
        var id = await layer.GetRegionIdAtAsync(5, 5);
        var result = await layer.TileBelongsToRegionAsync(5, 5, id);
        Assert.True(result);
    }

    [Fact]
    public async Task GetRegionsInArea_ReturnsCorrectSet()
    {
        var layer = new RegionLayer();
        await layer.GenerateRegionsAsync(100, 100, 4);
        var regions = await layer.GetRegionsInAreaAsync(0, 0, 99, 99);
        Assert.Equal(4, regions.Count);
    }
}
