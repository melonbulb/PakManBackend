namespace PakManBackend.Models;

public class Player : Sprite
{
    private int score;
    private int lives;

    public Player(string name, string img, Map.Position position) : base(name, img, position)
    {
        score = 0;
        lives = 3;
    }

    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    public int Lives
    {
        get { return lives; }
        set { lives = value; }
    }
}