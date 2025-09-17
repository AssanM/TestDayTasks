using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tileWorld.application.Services;
using tileWorld.domain.Entities;

namespace tileWorld.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObjectsController : ControllerBase
    {
        private readonly ObjectRepository _service;

        public ObjectsController(ObjectRepository service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> Add(MapObject obj)
        {
            await _service.AddObjectAsync(obj);
            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<MapObject>> Get(string id)
        {
            var obj = await _service.GetByIdAsync(id);
            return obj == null ? NotFound() : Ok(obj);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.RemoveAsync(id);
            return Ok();
        }

        [HttpGet("point")]
        public async Task<ActionResult<List<MapObject>>> GetByPoint([FromQuery] int x, [FromQuery] int y)
        {
            var list = await _service.GetByPointAsync(x, y);
            return Ok(list);
        }

        [HttpGet("area")]
        public async Task<ActionResult<List<MapObject>>> GetByArea([FromQuery] int x0, int y0, int x1, int y1)
        {
            var list = await _service.GetByAreaAsync(x0, y0, x1, y1);
            return Ok(list);
        }
        [HttpPut]
        public async Task<IActionResult> Update(MapObject obj)
        {
            await _service.UpdateAsync(obj);
            return Ok();
        }
    }
}
