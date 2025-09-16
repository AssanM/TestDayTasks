using testDay.application.Services;
using testDay.domain.Interfaces;

namespace testDay.application.Interfaces;

public interface ITileMapPersistence
{
    Task SaveEngineASync(IEngineLayer engine, string filePath);
    Task<EngineLayer> LoadEngingeAsync(string filePath);
}
