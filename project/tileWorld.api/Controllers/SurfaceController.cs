using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tileWorld.application.Models;
using tileWorld.domain.Interfaces;
using tileWorld.domain.ValueObjects;

namespace tileWorld.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurfaceController : ControllerBase
    {
        private readonly ISurfaceLayer _service;

        public SurfaceController(ISurfaceLayer service)
        {
            _service = service;
        }
        /// <summary>
        /// Получение типа поверхности тайла
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [HttpGet("{x:int}/{y:int}")]
        public async Task<ActionResult<SurfaceType>> GetTile(int x, int y)
        {
            if (!_service.IsInBounds(x, y))
                return BadRequest("Out of bounds");
            var type = await _service.GetTileAsync(x, y);
            return Ok(type);
        }

        /// <summary>
        /// Установить тип поверхности тайла
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost("{x:int}/{y:int}")]
        public async Task<IActionResult> SetTile(int x, int y, [FromBody] SurfaceType type)
        {
            if (!_service.IsInBounds(x, y))
                return BadRequest("Out of bounds");
            await _service.SetTileAsync(x, y, type);
            return Ok();
        }
        /// <summary>
        /// Заполнение облости тайлами одного типа
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("fill")]
        public async Task<IActionResult> FillArea([FromBody] FillRequest request)
        {

            await _service.FillArea(request.XStart, request.YStart, request.XEnd, request.YEnd, request.Type);
            return Ok();
        }
        /// <summary>
        /// Проверить возможность размещения объекта
        /// </summary>
        /// <param name="xStart"></param>
        /// <param name="yStart"></param>
        /// <param name="xEnd"></param>
        /// <param name="yEnd"></param>
        /// <returns></returns>
        [HttpGet("can-place")]
        public async Task<ActionResult<bool>> CanPlaceObject([FromQuery] int xStart, [FromQuery] int yStart, [FromQuery] int xEnd, [FromQuery] int yEnd)
        {
            var result = await _service.CanPlaceObjectInArea(xStart, yStart, xEnd, yEnd);
            return Ok(result);
        }
    }
}
