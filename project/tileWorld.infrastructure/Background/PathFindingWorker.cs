using Microsoft.Extensions.Hosting;
using tileWorld.application.Services;

namespace tileWorld.infrastructure.Background;

public class PathFindingWorker : BackgroundService
{
    private readonly PathFindingQueue _queue;

    public PathFindingWorker(PathFindingQueue queue) => _queue = queue;
    protected override async Task ExecuteAsync(CancellationToken cancellationToken) =>
        await _queue.StartProcessingAsync(cancellationToken);
}