using static System.Numerics.Vector;
using System.Drawing;
using ChessLine_Miami.Logic;
namespace ChessLine_Miami.Models;

public class Level
{
    public readonly string Name;
    public readonly string[] Maplines;
    public readonly Size Size;
    
    public readonly HashSet<Point> Walls;
    
    public readonly HashSet<Point> Lava;
    
    // Точки спавна (статические данные уровня)
    public readonly Point PlayerSpawn;
    public readonly List<Enemy> EnemySpawns;

    // Статическое поле (стены и лава) - не содержит динамических объектов
    public SectorType[,] Field;

    public Level(string name, Size size, HashSet<Point> walls, HashSet<Point> lava, Point playerSpawn, List<Enemy> enemySpawns, String[] maplines)
    {
        
        Name = name;
        Maplines = maplines;
        Size = size;
        
        Walls = walls;
        
        Lava = lava;
        
        PlayerSpawn = playerSpawn;
        EnemySpawns = enemySpawns;
        
        Field = new SectorType[size.Width, size.Height];
        
        CreateField();
        
    }

    public void CreateField()
    {
        for (int y = 0; y < Size.Height; y++)
        {
            for (int x = 0; x < Size.Width; x++)
            { 
                Field[x, y] = SectorType.Empty;
            }
        }
        //Field[Player.Pos.X, Player.Pos.Y] = SectorType.Player;
        foreach (var wall in Walls)
        {
            Field[wall.X, wall.Y] = SectorType.Wall;
        }
        foreach (var l in Lava)
        {
            Field[l.X, l.Y] = SectorType.Lava;
        }
        
        // foreach (var enemy in Enemies)
        // {
        //     Field[enemy.Pos.X, enemy.Pos.Y] = SectorType.Enemy;
        // }
        
    }

    public SectorType GetSector(int x, int y) => Field[x, y];
}
