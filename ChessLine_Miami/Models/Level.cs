using static System.Numerics.Vector;
using System.Drawing;
namespace ChessLine_Miami.Models;

public class Level
{
    public readonly string Name;
    public readonly Size Size;
    
    public readonly HashSet<Point> Walls;
    
    public readonly HashSet<Point> Lava;
    
    public Player Player;
    
    public List<Enemy> Enemies;

    public Level(string name, Size size, HashSet<Point> walls, HashSet<Point> lava, Player player, List<Enemy> enemies)
    { 
        Name = name;
        Size = size;
        Walls = walls;
        Lava = lava;
        Player = player;
        Enemies = enemies;
    }
    
    
    
}
