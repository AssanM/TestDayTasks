namespace testDay3.domain.Entities;

public class Region
{
    public ushort Id { get; }
    public string Name { get; set; }
    public Region(ushort id, string name)
    {
        Id = id;
        Name = name;
    }
}
