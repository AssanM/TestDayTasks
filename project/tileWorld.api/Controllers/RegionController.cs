using Microsoft.AspNetCore.Mvc;
using tileWorld.domain.Entities;
using tileWorld.domain.Interfaces;

namespace tileWorld.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly IRegionLayer _layer;

        public RegionController(IRegionLayer layer)
        {
            _layer = layer;
        }
        [HttpPost("generate")]
        public async Task<IActionResult> Generate(int width, int height, int count)
        {
            await _layer.GenerateRegionsAsync(width, height, count);
            return Ok();
        }

        [HttpGet("tile")]
        public async Task<ActionResult<ushort>> GetRegionIdAt(int x, int y)
        {
            var id = await _layer.GetRegionIdAtAsync(x, y);
            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Region>> GetRegionById(ushort id)
        {
            var region = await _layer.GetRegionByIdAsync(id);
            return Ok(region);
        }

        [HttpGet("area")]
        public async Task<ActionResult<List<Region>>> GetRegionsInArea(int x0, int y0, int x1, int y1)
        {
            var regions = await _layer.GetRegionsInAreaAsync(x0, y0, x1, y1);
            return Ok(regions);
        }

    }
}
