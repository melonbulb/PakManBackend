namespace PakManBackend;

using PakManBackend.Models;

class Program
{
    static void Main(string[] args)
    {
        Menu();
    }

    static void ManageMapMenu()
    {
        while (true)
        { }

    }

    static void ManageEnemyMenu(Map map)
    {
        while (true)
        {
            Console.WriteLine("---------------------------");
            Console.WriteLine("Manage Enemy Menu");
            Console.WriteLine("---------------------------");
            Console.WriteLine("1. List Enemies");
            Console.WriteLine("2. Add Enemy");
            Console.WriteLine("3. Update Enemy Position");
            Console.WriteLine("4. Remove Enemy");
            Console.WriteLine("0. Return to Previous Menu");
            Console.Write("Select an option: ");
            int.TryParse(Console.ReadLine(), out int choice);
            Console.Clear();

            switch (choice)
            {
                case 1:
                    if (map.Enemies.Count == 0)
                    {
                        Console.WriteLine("No enemies found on the map.");
                    }
                    else
                    {
                        for (int index = 0; index < map.Enemies.Count; index++)
                        {
                            var enemy = map.Enemies[index];
                            Console.WriteLine($"{index + 1}. {enemy.Name} at ({enemy.Position.X}, {enemy.Position.Y})");
                        }
                    }
                    break;
                case 2:
                    Console.WriteLine("Enter Enemy Name: ");
                    string name = Console.ReadLine()?.Trim() ?? "";
                    if (string.IsNullOrEmpty(name))
                    {
                        Console.WriteLine("Invalid name for enemy.");
                        break;
                    }
                    Console.WriteLine("Enter Enemy Image Path: ");
                    string imgPath = Console.ReadLine()?.Trim() ?? "";
                    if (string.IsNullOrEmpty(imgPath))
                    {
                        Console.WriteLine("Invalid image path for enemy.");
                        break;
                    }
                    Console.Write("Enter new X position for enemy: ");
                    int.TryParse(Console.ReadLine(), out int x);
                    Console.Write("Enter new Y position for enemy: ");
                    int.TryParse(Console.ReadLine(), out int y);
                    if (map.Graph.AdjacencyList.ContainsKey(new Map.Position(x, y)) == false)
                    {
                        Console.WriteLine("Invalid position for enemy.");
                    }
                    else
                    {
                        map.AddEnemy(new Enemy(name, imgPath, new Map.Position(x, y)));
                        Console.WriteLine("Enemy added to the map.");
                    }
                    break;
                case 3:
                    for (int index = 0; index < map.Enemies.Count; index++)
                    {
                        var enemy = map.Enemies[index];
                        Console.WriteLine($"{index + 1}. {enemy.Name} at ({enemy.Position.X}, {enemy.Position.Y})");
                    }
                    Console.Write("Enter Enemy Index to Update (starting from 1): ");
                    int.TryParse(Console.ReadLine(), out int enemyIndex);
                    enemyIndex -= 1; // convert to 0-based index
                    if (enemyIndex < 0 || enemyIndex >= map.Enemies.Count)
                    {
                        Console.WriteLine("Invalid enemy index.");
                        break;
                    }
                    Console.Write("Enter new X position for enemy: ");
                    int.TryParse(Console.ReadLine(), out int newX);
                    Console.Write("Enter new Y position for enemy: ");
                    int.TryParse(Console.ReadLine(), out int newY);
                    if (map.Graph.AdjacencyList.ContainsKey(new Map.Position(newX, newY)) == false)
                    {
                        Console.WriteLine("Invalid position for enemy.");
                    }
                    else
                    {
                        map.Enemies[enemyIndex].Position = new Map.Position(newX, newY);
                        Console.WriteLine("Enemy position updated.");
                    }
                    break;
                case 4:
                    for (int index = 0; index < map.Enemies.Count; index++)
                    {
                        var enemy = map.Enemies[index];
                        Console.WriteLine($"{index + 1}. {enemy.Name} at ({enemy.Position.X}, {enemy.Position.Y})");
                    }
                    Console.Write("Enter Enemy Index to Remove (starting from 1): ");
                    int.TryParse(Console.ReadLine(), out int removeIndex);
                    removeIndex -= 1; // convert to 0-based index
                    if (removeIndex < 0 || removeIndex >= map.Enemies.Count)
                    {
                        Console.WriteLine("Invalid enemy index.");
                        break;
                    }
                    else
                    {
                        if (map.RemoveEnemyAt(removeIndex))
                        {
                            Console.WriteLine("Enemy removed from the map.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to remove enemy from the map.");
                        }
                    }
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void ManagePlayerMenu(Map map)
    {
        while (true)
        {

            Console.WriteLine("---------------------------");
            Console.WriteLine("Manage Player Menu");
            Console.WriteLine("---------------------------");
            Console.WriteLine("1. Add Player");
            Console.WriteLine("2. Update Player Position");
            Console.WriteLine("3. Remove Player");
            Console.WriteLine("0. Return to Previous Menu");
            Console.Write("Select an option: ");
            int.TryParse(Console.ReadLine(), out int choice);
            Console.Clear();

            switch (choice)
            {
                case 1:
                    if (map.Player != null)
                    {
                        Console.WriteLine("Player found on the map.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Enter Player Name: ");
                        string name = Console.ReadLine()?.Trim() ?? "";
                        if (string.IsNullOrEmpty(name))
                        {
                            Console.WriteLine("Invalid name for player.");
                            break;
                        }
                        Console.WriteLine("Enter Player Image Path: ");
                        string imgPath = Console.ReadLine()?.Trim() ?? "";
                        if (string.IsNullOrEmpty(imgPath))
                        {
                            Console.WriteLine("Invalid image path for player.");
                            break;
                        }
                        Console.Write("Enter new X position for player: ");
                        int.TryParse(Console.ReadLine(), out int x);
                        Console.Write("Enter new Y position for player: ");
                        int.TryParse(Console.ReadLine(), out int y);
                        if (map.SetPlayer(new Player(name, imgPath, new Map.Position(x, y))) == false)
                        {
                            Console.WriteLine("Invalid position for player.");
                        }
                        else
                        {
                            Console.WriteLine("Player added to the map.");
                        }
                    }
                    break;
                case 2:
                    if (map.Player == null)
                    {
                        Console.WriteLine("No player found on the map.");
                        break;
                    }
                    else
                    {
                        Console.Write("Enter new X position for player: ");
                        int.TryParse(Console.ReadLine(), out int x);
                        Console.Write("Enter new Y position for player: ");
                        int.TryParse(Console.ReadLine(), out int y);
                        if (map.UpdatePlayerPosition(Position(x, y)) == false)
                        {
                            Console.WriteLine("Invalid position for player.");
                        }
                        else
                        {
                            Console.WriteLine("Player position updated.");
                        }
                    }
                    break;
                case 3:
                    if (map.Player == null)
                    {
                        Console.WriteLine("No player found on the map.");
                        break;
                    }
                    else
                    {
                        if (map.RemovePlayer())
                        {
                            Console.WriteLine("Player removed from the map.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to remove player from the map.");
                        }
                    }
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void PrintReport(Map map)
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("Map Report");
        Console.WriteLine("---------------------------");
        Console.WriteLine($"Map Size: {map.Rows} x {map.Columns}");
        Console.WriteLine($"Total Tiles: {map.Columns * map.Rows}");
        Console.WriteLine($"Wall Tiles: {map.GetMap().SelectMany(row => row).Count(t => t == 1)}");
        Console.WriteLine($"Food Tiles: {map.GetMap().SelectMany(row => row).Count(t => t == 2)}");
        Console.WriteLine($"Power-Up Tiles: {map.GetMap().SelectMany(row => row).Count(t => t == 3)}");
        Console.WriteLine($"Player Start Position: ({map.Player?.Position.X}, {map.Player?.Position.Y})");
        if (map.Enemies.Count > 0)
        {
            for (int i = 0; i < map.Enemies.Count; i++)
            {
                Console.WriteLine($"Enemy {map.Enemies[i].Name} Start Position: ({map.Enemies[i].Position.X}, {map.Enemies[i].Position.Y})");
            }
        }
        else
        {
            Console.WriteLine("No Enemies on the Map.");
        }
    }

    static void SubMenu(Map map)
    {
        if (map != null)
        {
            Console.WriteLine("Map generated successfully.");
            while (true)
            {
                Console.WriteLine("---------------------------");
                Console.WriteLine("Map Configuration Menu");
                Console.WriteLine("---------------------------");
                Console.WriteLine("1. View Map (Graphical)");
                Console.WriteLine("2. View Map (Numeric)");
                Console.WriteLine("3. View Map Report");
                Console.WriteLine("4. View Map JSON Configuration");
                Console.WriteLine("5. View Enemies to Player Paths");
                Console.WriteLine("6. Manage Player");
                Console.WriteLine("7. Manage Enemies");
                Console.WriteLine("0. Exit to Main Menu");
                Console.WriteLine("Select an option: ");
                int.TryParse(Console.ReadLine(), out int choice);
                Console.Clear();
                switch (choice)
                {
                    case 1:
                        map.PrintMap("graphical");
                        break;
                    case 2:
                        map.PrintMap();
                        break;
                    case 3:
                        PrintReport(map);
                        break;
                    case 4:
                        Console.WriteLine("---------------------------");
                        Console.WriteLine("Map JSON:");
                        Console.WriteLine("---------------------------");
                        Console.WriteLine(map.Config);
                        break;
                    case 5:
                        map.PrintEnemiesToPlayerPaths();
                        break;
                    case 6:
                        ManagePlayerMenu(map);
                        break;
                    case 7:
                        ManageEnemyMenu(map);
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
                Console.WriteLine();
            }
        }
    }

    static Map GenerateRandomMap()
    {
        int rows = 0, columns = 0;
        while (true)
        {
            Console.WriteLine("---------------------------");
            Console.WriteLine("Random Map Generation");
            Console.WriteLine("---------------------------");
            Console.Write("Enter number of columns for the map: ");
            if (!int.TryParse(Console.ReadLine(), out columns))
            {
                Console.WriteLine("Invalid input for columns");
                continue;
            }
            Console.Write("Enter number of rows for the map: ");
            if (!int.TryParse(Console.ReadLine(), out rows))
            {
                Console.WriteLine("Invalid input for rows");
                continue;
            }
            break;
        }
        var map = new Map(columns, rows);
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                double rand = new Random().NextDouble();
                if (rand < 0.2)
                {
                    map.SetTile(Position(col, row), "wall");
                }
                else if (rand < 0.6)
                {
                    map.SetTile(Position(col, row), "power-up");
                }
            }
        }
        map.GenerateGraph();
        return map;
    }

    static void Menu()
    {
        while (true)
        {
            Console.WriteLine("---------------------------");
            Console.WriteLine("Map Generator Menu");
            Console.WriteLine("---------------------------");
            Console.WriteLine("1. Generate Custom Map 1");
            Console.WriteLine("2. Generate Random Map");
            Console.WriteLine("0. Exit");
            Console.Write("Select an option: ");
            int.TryParse(Console.ReadLine(), out int choice);
            Console.Clear();
            Map map;
            switch (choice)
            {
                case 1:
                    map = GenerateCustomMap1();
                    SubMenu(map);
                    break;
                case 2:
                    map = GenerateRandomMap();
                    SubMenu(map);
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
            Console.WriteLine();
        }


    }

    static Map GenerateCustomMap1()
    {
        Map map = new Map(20, 15);
        // draw walls
        // draw border walls
        map.SetTile(Position(0, 0), Position(19, 0), "wall");
        map.SetTile(Position(19, 0), Position(19, 14), "wall");
        map.SetTile(Position(19, 14), Position(0, 14), "wall");
        map.SetTile(Position(0, 14), Position(0, 0), "wall");

        // create entrances at the border walls
        map.SetTile(Position(0, 6), Position(2, 6), "wall");
        map.SetTile(Position(0, 7), "food");
        map.SetTile(Position(0, 8), Position(2, 8), "wall");

        map.SetTile(Position(17, 6), Position(19, 6), "wall");
        map.SetTile(Position(19, 7), "food");
        map.SetTile(Position(17, 8), Position(19, 8), "wall");

        // draw first 3 square walls left side
        map.SetTile(Position(2, 2), Position(4, 4), "wall");
        map.SetTile(Position(2, 10), Position(4, 12), "wall");
        map.SetTile(Position(4, 6), Position(6, 8), "wall");

        // draw first 3 square walls right side
        map.SetTile(Position(15, 2), Position(17, 4), "wall");
        map.SetTile(Position(15, 10), Position(17, 12), "wall");
        map.SetTile(Position(13, 6), Position(15, 8), "wall");

        // top center upside down U shape
        map.SetTile(Position(6, 2), Position(6, 4), "wall");
        map.SetTile(Position(6, 2), Position(13, 2), "wall");
        map.SetTile(Position(13, 2), Position(13, 4), "wall");

        // U shape content
        map.SetTile(Position(8, 4), Position(11, 6), "wall");
        map.SetTile(Position(8, 8), Position(11, 8), "wall");

        // left center bottom rectangle
        map.SetTile(Position(6, 10), Position(7, 12), "wall");

        // right center bottom circular shape
        map.SetTile(Position(13, 12), Position(13, 10), "wall");
        map.SetTile(Position(13, 10), Position(9, 10), "wall");
        map.SetTile(Position(9, 10), Position(9, 12), "wall");
        map.SetTile(Position(9, 12), Position(11, 12), "wall");

        // draw power-ups
        map.SetTile(Position(1, 1), "power-up");
        map.SetTile(Position(1, 13), "power-up");
        map.SetTile(Position(18, 1), "power-up");
        map.SetTile(Position(18, 13), "power-up");
        map.SetTile(Position(9, 7), "power-up");
        map.SetTile(Position(10, 9), "power-up");
        map.SetTile(Position(5, 11), "power-up");
        map.SetTile(Position(14, 11), "power-up");
        map.SetTile(Position(14, 3), "power-up");

        map.GenerateGraph();

        map.SetPlayer(new Player("Player1", "player.png", Position(5, 1)));
        map.AddEnemy(new Enemy("Enemy1", "enemy.png", Position(14, 1)));
        map.AddEnemy(new Enemy("Enemy2", "enemy.png", Position(14, 13)));
        return map;
    }

    static Map.Position Position(int x, int y)
    {
        return new Map.Position(x, y);
    }
}
