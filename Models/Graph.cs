namespace PakManBackend.Models;

public class Graph
{
    public Dictionary<Map.Position, List<Map.Position>> AdjacencyList { get; private set; }

    public Graph()
    {
        AdjacencyList = new Dictionary<Map.Position, List<Map.Position>>();
    }

    public void AddEdge(Map.Position from, Map.Position to)
    {
        if (!AdjacencyList.ContainsKey(from))
        {
            AdjacencyList[from] = new List<Map.Position>();
        }
        AdjacencyList[from].Add(to);
    }

    public List<Map.Position> GetNeighbors(Map.Position position)
    {
        if (AdjacencyList.ContainsKey(position))
        {
            return AdjacencyList[position];
        }
        return new List<Map.Position>();
    }

    public bool HasEdge(Map.Position from, Map.Position to)
    {
        return AdjacencyList.ContainsKey(from) && AdjacencyList[from].Contains(to);
    }

    public void PrintGraph()
    {
        foreach (var kvp in AdjacencyList)
        {
            Console.Write(kvp.Key.ToString() + " -> ");
            Console.WriteLine(string.Join(", ", kvp.Value.Select(pos => pos.ToString())));
        }
    }


    public List<Map.Position> GetShortestPath(Map.Position from, Map.Position to)
    {
        var queue = new Queue<Map.Position>();
        var visited = new HashSet<Map.Position> { from };
        var predecessors = new Dictionary<Map.Position, Map.Position>();

        queue.Enqueue(from);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current.Equals(to))
                break;

            foreach (var neighbor in AdjacencyList[current])
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    // Set the predecessor of the neighbor to the current node
                    predecessors[neighbor] = current;
                    queue.Enqueue(neighbor);
                }
            }
        }

        // Reconstruct path
        var path = new List<Map.Position>();
        if (!visited.Contains(to))
        {
            // No path found
            return path;
        }

        // Build the path by backtracking from end to start
        var at = to;
        while (at != null)
        {
            path.Add(at);
            if (at.Equals(from))
                break;
            at = predecessors[at];
        }
        // Reverse the path to get it from start to end
        path.Reverse();

        return path;
    }

    public void PrintShortestPath(Map.Position from, Map.Position to)
    {
        var path = GetShortestPath(from, to);
        if (path.Count == 0)
        {
            Console.WriteLine("No path found from " + from.ToString() + " to " + to.ToString());
        }
        else
        {
            Console.WriteLine("Shortest path from " + from.ToString() + " to " + to.ToString() + ":");
            Console.WriteLine(string.Join(" -> ", path.Select(pos => pos.ToString())));
        }
    }
}