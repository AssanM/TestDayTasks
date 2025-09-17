using Microsoft.AspNetCore.Mvc;
using testDay4.application.Services;
using testDay4.domain.Entities;

namespace testDay4.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ObjectController : ControllerBase
{
    private readonly ObjectService _service;

    public ObjectController(ObjectService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult Add(MapObject obj)
    {
        _service.Add(obj);
        return Ok();
    }

    [HttpPut]
    public IActionResult Update(MapObject obj)
    {
        _service.Update(obj);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        _service.Delete(id);
        return Ok();
    }

    [HttpGet("area")]
    public async Task<ActionResult<List<MapObject>>> GetByArea(int x0, int y0, int x1, int y1)
    {
        var result = await _service.GetByAreaAsync(x0, y0, x1, y1);
        return Ok(result);
    }

}
