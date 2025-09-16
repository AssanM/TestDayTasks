namespace testDay.application.Models;

public class PathFindingTaskStatus
{
    public Guid TaskId { get; set; }
    public float Progress { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsFailed { get; set; }
    public List<(int x, int y)> Result { get; set; } = new();

}
