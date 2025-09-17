namespace tileWorld.domain.Entities;

public class MapObject
{
    public string Id { get; }
    public int X { get; }
    public int Y { get; }
    public int Width { get; }
    public int Height { get; }

    public MapObject(string id, int x, int y, int width, int height)
    {
        Id = id;
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public bool Intersects(int x0, int y0, int x1, int y1) =>
        !(X + Width <= x0 || X >= x1 || Y + Height <= y0 || Y >= y1);
    public bool Contains(int px, int py) =>
       px >= X && px < X + Width && py >= Y && py < Y + Height;
}
