namespace testDay.domain.Entities;

public class Node
{
    public int X { get; set; }
    public int Y { get; set; }
    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost => GCost + HCost;
    public Node Parent { get; set; }
    public Node(int x, int y) => (X, Y) = (x, y);

    public override bool Equals(object? obj) => obj is Node node && node.X == X && node.Y == Y;
    public override int GetHashCode() => HashCode.Combine(X, Y);
}
