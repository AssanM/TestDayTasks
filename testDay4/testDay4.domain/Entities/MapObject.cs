namespace testDay4.domain.Entities;

public class MapObject
{
    public string Id { get; }
    public int X { get; private set; }
    public int Y { get; private set; }
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

    public bool Contains(int px, int py) =>
        px >= X && px < X + Width && py >= Y && py < Y + Height;

    public bool Intersects(int x0, int y0, int x1, int y1) =>
        !(X + Width <= x0 || X >= x1 || Y + Height <= y0 || Y >= y1);
}
