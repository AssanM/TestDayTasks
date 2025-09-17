using testDay4.domain.Entities;

namespace testDay4.domain.Interfaces;

public interface IRegionLayer
{
    /// <summary>Генерация регионов для карты заданного размера</summary>
    Task GenerateRegionsAsync(int mapWidth, int mapHeight, int regionCount);

    /// <summary>Получить ID региона для тайла (x, y)</summary>
    Task<ushort> GetRegionIdAtAsync(int x, int y);

    /// <summary>Получить метаданные региона по ID</summary>
    Task<Region> GetRegionByIdAsync(ushort id);

    /// <summary>Проверить, принадлежит ли тайл региону</summary>
    Task<bool> TileBelongsToRegionAsync(int x, int y, ushort regionId);

    /// <summary>Получить все регионы, пересекающие указанную область</summary>
    Task<List<Region>> GetRegionsInAreaAsync(int x0, int y0, int x1, int y1);

}
