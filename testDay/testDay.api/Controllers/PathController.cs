using Microsoft.AspNetCore.Mvc;
using testDay.application.Models;
using testDay.application.Services;
using testDay.domain.Interfaces;

namespace testDay.api.Controllers;

[ApiController]
[Route("api/path")]
public class PathController : ControllerBase
{
    private readonly PathFindingQueue _queue;
    private readonly IEngineLayer _engine;

    public PathController(PathFindingQueue queue, IEngineLayer engine)
    {
        _queue = queue;
        _engine = engine;
    }
    /// <summary>
    /// Получение taskId
    /// </summary>
    /// <param name="startX"></param>
    /// <param name="startY"></param>
    /// <param name="endX"></param>
    /// <param name="endY"></param>
    /// <returns></returns>
    [HttpPost("start")]
    public ActionResult<Guid> StartPathFinding(
        [FromQuery] int startX,
        [FromQuery] int startY,
        [FromQuery] int endX,
        [FromQuery] int endY)
    {
        var taskId = _queue.Enqueue(async (progress, token) =>
        {
            var layer = new PathFindingLayer(_engine);
            return await layer.FindPathAsync(startX, startY, endX, endY, token, progress);
        });
        return Ok(taskId);
    }
    /// <summary>
    /// Проверка статуса по TaskId
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    [HttpGet("status/{taskId}")]
    public ActionResult<PathFindingTaskStatus> GetStatus(Guid taskId)
    {
        var status = _queue.GetStatus(taskId);
        if (status == null) return NotFound();
        return Ok(status);
    }
}
