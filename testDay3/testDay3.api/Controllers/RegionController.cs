using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testDay3.domain.Entities;
using testDay3.domain.Interfaces;

namespace testDay3.api.Controllers
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
        public async Task<IActionResult> Generate([FromQuery] int width, int height, int count)
        {
            await _layer.GenerateRegionsAsync(width, height, count);
            return Ok();
        }

        [HttpGet("tile")]
        public async Task<ActionResult<ushort>> GetRegionIdAt([FromQuery] int x, int y)
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

        [HttpGet("belongs")]
        public async Task<ActionResult<bool>> TileBelongs([FromQuery] int x, int y, ushort regionId)
        {
            var result = await _layer.TileBelongsToRegionAsync(x, y, regionId);
            return Ok(result);
        }

        [HttpGet("area")]
        public async Task<ActionResult<List<Region>>> GetRegionsInArea([FromQuery] int x0, int y0, int x1, int y1)
        {
            var regions = await _layer.GetRegionsInAreaAsync(x0, y0, x1, y1);
            return Ok(regions);
        }

    }
}
