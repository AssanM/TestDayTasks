using testDay.domain.ValueObjects;

namespace testDay.domain.Entities;

public class EngineTile
{
    public EngineType Type { get; set; }

    public EngineTile(EngineType type)
    {
        Type = type;
    }
    public bool CanPlaceObject => Type == EngineType.Plain;
    public void SetType(EngineType type) => Type = type;
}
