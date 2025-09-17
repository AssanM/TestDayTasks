using System.Collections.Concurrent;
using System.Threading.Channels;
using tileWorld.application.Models;

namespace tileWorld.application.Services;

public class PathFindingQueue
{
    private readonly ConcurrentDictionary<Guid, PathFindingTaskStatus> _tasks = new();
    private readonly Channel<(Guid, Func<IProgress<float>, CancellationToken, Task<List<(int, int)>>>)> _channel = Channel.CreateUnbounded<(Guid, Func<IProgress<float>, CancellationToken, Task<List<(int, int)>>>)>();

    public Guid Enqueue(Func<IProgress<float>, CancellationToken, Task<List<(int, int)>>> taskFunc)
    {
        var taskId = Guid.NewGuid();
        _tasks[taskId] = new PathFindingTaskStatus { TaskId = taskId };
        _channel.Writer.TryWrite((taskId, taskFunc));
        return taskId;
    }

    public PathFindingTaskStatus GetStatus(Guid taskId) =>
        _tasks.TryGetValue(taskId, out var status) ? status : null;

    public async Task StartProcessingAsync(CancellationToken token)
    {
        await foreach (var (taskId, taskFunc) in _channel.Reader.ReadAllAsync(token))
        {
            var status = _tasks[taskId];
            try
            {
                var progress = new Progress<float>(p => status.Progress = p);
                var result = await taskFunc(progress, token);
                status.Result = result;
                status.IsCompleted = true;
            }
            catch
            {
                status.IsFailed = true;
            }
        }
    }

}
