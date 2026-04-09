namespace ChessLine_Miami.Models;

public class Enemy
{
    public EnemyType Type;
    public Point Pos;
    public bool IsAlive;

    public Enemy(Point p, EnemyType type)
    {
        Pos = p;
        Type = type;
    }
}