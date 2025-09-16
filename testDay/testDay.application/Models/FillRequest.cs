using testDay.domain.ValueObjects;

namespace testDay.application.Models;

public class FillRequest
{
    public int XStart { get; set; }
    public int YStart { get; set; }
    public int XEnd { get; set; }
    public int YEnd { get; set; }
    public EngineType Type { get; set; }
}
