using Microsoft.AspNetCore.Mvc;
using tileWorld.domain.Entities;
using tileWorld.domain.Interfaces;

namespace tileWorld.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObjectController : ControllerBase
    {
        private readonly IObjectLayer _layer;

        public ObjectController(IObjectLayer layer)
        {
            _layer = layer;
        }
        [HttpPost]
        public async Task<IActionResult> Add(MapObject obj)
        {
            await _layer.AddAsync(obj);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(MapObject obj)
        {
            await _layer.UpdateAsync(obj);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _layer.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("area")]
        public async Task<ActionResult<List<MapObject>>> GetByArea(int x0, int y0, int x1, int y1)
        {
            var result = await _layer.GetByAreaAsync(x0, y0, x1, y1);
            return Ok(result);
        }

    }
}
