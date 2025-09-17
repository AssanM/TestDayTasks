using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tileWorld.application.Models;
using tileWorld.application.Services;
using tileWorld.domain.Interfaces;

namespace tileWorld.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PathController : ControllerBase
    {
        private readonly PathFindingQueue _queue;
        private readonly ISurfaceLayer _engine;

        public PathController(PathFindingQueue queue, ISurfaceLayer engine)
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
}
