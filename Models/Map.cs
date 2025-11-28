using System.Text.Json;
namespace PakManBackend.Models;

public class Map
{
    private readonly int columns, rows;
    private int[][] map;
    private Player? player;
    private List<Enemy> enemies = new List<Enemy>();
    private int enemyCount, powerUpCount, foodCount;
    private Dictionary<Position, List<Position>>? graph;

    public record Position(int X, int Y);
    public int Columns => columns;
    public int Rows => rows;
    public int EnemyCount => enemyCount;
    public int ConsumablesCount => powerUpCount + foodCount;

    public string Config =>
        JsonSerializer.Serialize(new
        {
            columns,
            rows,
            enemyCount,
            powerUpCount,
            foodCount,
            map
        });

    public Map(int columns, int rows)
    {
        this.columns = columns;
        this.rows = rows;
        powerUpCount = 0;
        foodCount = 0;
        enemyCount = 0;
        map = new int[rows][];
        for (int y = 0; y < rows; y++)
        {
            map[y] = new int[columns];
            for (int x = 0; x < columns; x++)
            {
                SetTile(new Position(x, y), "food", false);
            }
        }
    }

    public Dictionary<Position, List<Position>> GenerateGraph()
    {
        Graph graph = new Graph();
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Position currentPos = new Position(x, y);
                if (GetTileType(currentPos) != "wall")
                {
                    // Check all four directions
                    Position[] directions =
                    {
                            new Position(x, y - 1), // Up
                            new Position(x, y + 1), // Down
                            new Position(x - 1, y), // Left
                            new Position(x + 1, y)  // Right
                        };

                    foreach (var dir in directions)
                    {
                        if (dir.X >= 0 && dir.X < columns && dir.Y >= 0 && dir.Y < rows)
                        {
                            if (GetTileType(dir) != "wall")
                            {
                                graph.AddEdge(currentPos, dir);
                            }
                        }
                    }
                }
            }
        }
        this.graph = graph.AdjacencyList;
        return graph.AdjacencyList;
    }

    public Dictionary<Position, List<Position>> Graph
    {
        get
        {
            if (graph == null)
            {
                return GenerateGraph();
            }
            return graph;
        }
    }

    public Player? Player
    {
        get { return player; }
    }

    public List<Enemy> Enemies
    {
        get { return enemies; }
    }

    public bool AddEnemy(Enemy newEnemy)
    {
        if (graph == null)
        {
            throw new InvalidOperationException("Graph not generated. Call GenerateGraph() before setting the player.");
        }
        if (graph.ContainsKey(newEnemy.Position) == false)
        {
            return false;
        }
        enemies.Add(newEnemy);
        enemyCount++;
        PrintMessage("Enemy added at position (" + newEnemy.Position.X + ", " + newEnemy.Position.Y + ")", "info");
        return true;
    }

    public bool RemoveEnemyAt(int index)
    {
        if (index >= 0 && index < enemies.Count)
        {
            enemies.RemoveAt(index);
            enemyCount--;
            return true;
        }
        return false;
    }

    public bool SetPlayer(Player newPlayer)
    {
        if (graph == null)
        {
            throw new InvalidOperationException("Graph not generated. Call GenerateGraph() before setting the player.");
        }
        if (graph.ContainsKey(newPlayer.Position) == false)
        {
            PrintMessage("Invalid position for player.", "warning");
            return false;
        }
        player = newPlayer;
        RemoveTile(newPlayer.Position);
        PrintMessage("Player set at position (" + newPlayer.Position.X + ", " + newPlayer.Position.Y + ")", "info");
        return true;
    }

    public bool UpdatePlayerPosition(Position newPos)
    {
        if (graph == null)
        {
            throw new InvalidOperationException("Graph not generated. Call GenerateGraph() before setting the player.");
        }
        if (graph.ContainsKey(newPos) == false)
        {
            return false;
        }
        if (player != null)
        {
            player.Position = newPos;
            RemoveTile(newPos);
            return true;
        }
        return false;
    }

    public bool RemovePlayer()
    {
        if (player != null)
        {
            player = null;
            return true;
        }
        return false;
    }

    public void SetTile(Position pos, string tile, bool removeFirst = true)
    {
        {
            int x = pos.X;
            int y = pos.Y;
            if (removeFirst)
            {
                RemoveTile(new Position(x, y));
            }
            switch (tile)
            {
                case "empty":
                    PrintMessage("Setting empty tile at position (" + x + ", " + y + ")", "info");
                    map[y][x] = 0;
                    break;
                case "wall":
                    PrintMessage("Setting wall at position (" + x + ", " + y + ")", "info");
                    map[y][x] = 1;
                    break;
                case "food":
                    PrintMessage("Setting food at position (" + x + ", " + y + ")", "info");
                    map[y][x] = 2;
                    foodCount++;
                    break;
                case "power-up":
                    PrintMessage("Setting power-up at position (" + x + ", " + y + ")", "info");
                    map[y][x] = 3;
                    powerUpCount++;
                    break;
                default:
                    throw new ArgumentException("Invalid tile type");
            }
        }
    }

    public void SetTile(Position pos1, Position pos2, string tile)
    {

        int x1 = Math.Min(pos1.X, pos2.X);
        int y1 = Math.Min(pos1.Y, pos2.Y);
        int x2 = Math.Max(pos1.X, pos2.X);
        int y2 = Math.Max(pos1.Y, pos2.Y);
        for (int x = x1; x <= x2; x++)
        {
            for (int y = y1; y <= y2; y++)
            {
                SetTile(new Position(x, y), tile);
            }
        }
    }

    public string GetTileType(Position pos)
    {
        int x = pos.X;
        int y = pos.Y;
        int tile = map[y][x];
        return tile switch
        {
            0 => "empty",
            1 => "wall",
            2 => "food",
            3 => "power-up",
            _ => "unknown",
        };
    }

    public void RemoveTile(Position pos)
    {
        int x = pos.X;
        int y = pos.Y;
        switch (GetTileType(pos))
        {
            case "empty":
                break;
            case "wall":
                PrintMessage("Removing wall at position (" + x + ", " + y + ")", "info");
                break;
            case "food":
                PrintMessage("Removing food at position (" + x + ", " + y + ")", "info");
                foodCount--;
                break;
            case "power-up":
                PrintMessage("Removing power-up at position (" + x + ", " + y + ")", "info");
                powerUpCount--;
                break;
            default:
                throw new ArgumentException("Invalid tile type");
        }
        map[y][x] = 0;
    }

    public int GetTile(Position pos)
    {
        int x = pos.X;
        int y = pos.Y;
        return map[y][x];
    }

    public Dictionary<Position, int> GetAdjacentTiles(Position pos)
    {
        int x = pos.X;
        int y = pos.Y;
        Dictionary<Position, int> adjacentTiles = new Dictionary<Position, int>();
        Position[] directions = new Position[]
        {
                new Position(x, y - 1), // Up
                new Position(x, y + 1), // Down
                new Position(x - 1, y), // Left
                new Position(x + 1, y)  // Right
        };
        foreach (var direction in directions)
        {
            int dx = direction.X;
            int dy = direction.Y;
            if (dx >= 0 && dx < columns && dy >= 0 && dy < rows)
            {
                adjacentTiles[direction] = GetTile(direction);
            }
        }
        return adjacentTiles;
    }

    public int[][] GetMap()
    {
        return map;
    }

    public void PrintReport()
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("Map Report");
        Console.WriteLine("---------------------------");
        Console.WriteLine($"Dimensions: {columns} columns x {rows} rows");
        Console.WriteLine($"Total Tiles: {columns * rows}");
        Console.WriteLine($"Food Count: {foodCount}");
        Console.WriteLine($"Power-Up Count: {powerUpCount}");
        Console.WriteLine($"Enemy Count: {enemyCount}");
        if (player != null)
        {
            Console.WriteLine($"Player Start Position: ({player.Position.X}, {player.Position.Y})");
        }
        else
        {
            Console.WriteLine("Player: Not Set");
        }
        if (enemies != null)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                Console.WriteLine($"Enemy {enemies[i].Name} Start Position: ({enemies[i].Position.X}, {enemies[i].Position.Y})");
            }
        }
        else
        {
            Console.WriteLine("Enemies: Not Set");
        }
    }

    public void PrintMap(string type = "default")
    {
        Console.Write($"{" ",3}");
        for (int x = 0; x < columns; x++)
        {
            Console.Write($"{x,3}");
        }
        Console.WriteLine();
        for (int y = 0; y < rows; y++)
        {
            Console.Write($"{y,3}");
            for (int x = 0; x < columns; x++)
            {
                if (type == "graphical")
                {
                    switch (map[y][x])
                    {
                        case 0:
                            Console.Write($"{" ",3}");
                            break;
                        case 1:
                            Console.Write($"{"🧱",3}");
                            break;
                        case 2:
                            Console.Write($"{"🍇",3}");
                            break;
                        case 3:
                            Console.Write($"{"💪",3}");
                            break;
                    }
                }
                else
                {
                    Console.Write($"{map[y][x],3}");
                }
            }
            Console.WriteLine();
        }
    }
    private static void PrintMessage(string message, string type = "info")
    {
        switch (type)
        {
            case "info":
                Console.ForegroundColor = ConsoleColor.White;
                break;
            case "warning":
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;
            case "error":
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            default:
                Console.ResetColor();
                break;
        }
        Console.WriteLine("Map: " + message);
        Console.ResetColor();
    }
}