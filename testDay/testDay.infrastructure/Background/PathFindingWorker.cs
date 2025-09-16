using Microsoft.Extensions.Hosting;
using testDay.application.Services;

namespace testDay.infrastructure.Background;

public class PathFindingWorker : BackgroundService
{
    private readonly PathFindingQueue _queue;

    public PathFindingWorker(PathFindingQueue queue)=>_queue = queue;
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)=>
        await _queue.StartProcessingAsync(cancellationToken);
}
