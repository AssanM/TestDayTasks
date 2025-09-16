using System.Text.Json;
using testDay.application.Interfaces;
using testDay.application.Services;
using testDay.domain.Entities;
using testDay.domain.Interfaces;
using testDay.domain.ValueObjects;

namespace testDay.infrastructure.Persistence;

public class TileMapPersistence : ITileMapPersistence
{
    public async Task SaveEngineASync(IEngineLayer engine, string filePath)
    {
        var data = new byte[engine.Width * engine.Height];
        for(int y=0;y<engine.Height;y++)
            for (int x=0;x<engine.Width;x++)
            {
                var type = await engine.GetTileAsync(x, y);
                data[y * engine.Width + x] = (byte)type;
            }

        var map = new MapData
        {
            Width = engine.Width,
            Height = engine.Height,
            Tiles = data
        };
        var json = JsonSerializer.Serialize(map);
        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task<EngineLayer> LoadEngingeAsync(string filePath)
    {
        var json = await File.ReadAllTextAsync(filePath);
        var map = JsonSerializer.Deserialize<MapData>(json);

        var engine = new EngineLayer(map.Width, map.Height);
        for (int y=0;y<map.Width;y++)
            for (int x=0;x<map.Height;x++)
            {
                var type = (EngineType)map.Tiles[y * map.Width + x];
                await engine.SetTileAsync(x, y, type);
            }
        return engine;
    }
}
