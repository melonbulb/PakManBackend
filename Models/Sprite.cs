namespace PakManBackend.Models;

public class Sprite
{
    string name;
    string imagePath;

    Map.Position position;

    public Sprite(string name, string imagePath, Map.Position position)
    {
        this.name = name;
        this.imagePath = imagePath;
        this.position = position;
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public string ImagePath
    {
        get { return imagePath; }
        set { imagePath = value; }
    }

    public Map.Position Position
    {
        get { return position; }
        set { position = value; }
    }
}