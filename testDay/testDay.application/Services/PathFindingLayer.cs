using testDay.domain.Entities;
using testDay.domain.Interfaces;
using testDay.domain.ValueObjects;

namespace testDay.application.Services;

public class PathFindingLayer
{
    private readonly IEngineLayer _enginelayer;

    public PathFindingLayer(IEngineLayer enginelayer)=>_enginelayer = enginelayer;
    public async Task<List<(int x, int y)>> FindPathAsync(int startX, int startY, int endX, int endY, CancellationToken token, IProgress<float> progress)
    {
        return await Task.Run(async () =>
        {
            var openSet = new List<Node>();
            var closedSet = new HashSet<Node>();
            var startNode = new Node(startX, startY) { GCost = 0, HCost = Heuristic(startX, startY, endX, endY) };
            openSet.Add(startNode);

            int steps = 0;
            while (openSet.Count > 0)
            {
                token.ThrowIfCancellationRequested();
                steps++;
                progress?.Report(steps / 1000f);

                var current = openSet.OrderBy(n => n.FCost).First();
                if (current.X == endX && current.Y == endY)
                    return ReconstructPath(current);

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (var neighbor in GetNeighbors(current))
                {
                    if (closedSet.Contains(neighbor)) continue;
                    if (!_enginelayer.IsInBounds(neighbor.X, neighbor.Y)) continue;
                    var tile = await _enginelayer.GetTileAsync(neighbor.X, neighbor.Y);
                    if (tile != EngineType.Plain) continue;

                    int tentativeG = current.GCost + 1;
                    var existing = openSet.FirstOrDefault(n => n.Equals(neighbor));
                    if (existing == null)
                    {
                        neighbor.GCost = tentativeG;
                        neighbor.HCost = Heuristic(neighbor.X, neighbor.Y, endX, endY);
                        neighbor.Parent = current;
                        openSet.Add(neighbor);
                    }
                    else if (tentativeG < existing.GCost)
                    {
                        existing.GCost = tentativeG;
                        existing.Parent = current;
                    }
                }
            }

            return new List<(int, int)>();
        }, token);
    }

    private int Heuristic(int x1, int y1, int x2, int y2) => Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
    private List<Node> GetNeighbors(Node node) =>
        new() { new(node.X + 1, node.Y), new(node.X - 1, node.Y), new(node.X, node.Y + 1), new(node.X, node.Y - 1) };

    private List<(int x, int y)> ReconstructPath(Node end)
    {
        var path = new List<(int, int)>();
        var current = end;
        while (current != null)
        {
            path.Add((current.X, current.Y));
            current = current.Parent;
        }
        path.Reverse();
        return path;
    }

}
