using tileWorld.domain.Entities;
using tileWorld.domain.Interfaces;

namespace tileWorld.application.Services;

public class ObjectRepository
{
    private readonly IObjectRepository _repository;

    public ObjectRepository(IObjectRepository repository)
    {
        _repository = repository;
    }

    public Task AddObjectAsync(MapObject obj) => _repository.AddAsync(obj);
    public Task<MapObject?> GetByIdAsync(string id) => _repository.GetByIdAsync(id);
    public Task RemoveAsync(string id) => _repository.RemoveAsync(id);
    public Task<List<MapObject>> GetByPointAsync(int x, int y) => _repository.GetByPointAsync(x, y);
    public Task<List<MapObject>> GetByAreaAsync(int x0, int y0, int x1, int y1) => _repository.GetByAreaAsync(x0, y0, x1, y1);
    public Task UpdateAsync(MapObject obj) => _repository.UpdateAsync(obj);
}
