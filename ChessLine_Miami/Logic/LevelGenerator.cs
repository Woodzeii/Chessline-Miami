using ChessLine_Miami.Models;
namespace ChessLine_Miami.Logic;

public class LevelGenerator
{
    public static Level LoadFromStringArray(string[] mapLines)
    {
        var walls = new HashSet<Point>();
        var lava = new HashSet<Point>();
        var enemies = new List<Enemy>();
        Player player = null;

        int height = mapLines.Length;
        int width = mapLines[0].Length;
        Size room = new Size(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                char symbol = mapLines[y][x];
                Point pos = new Point(x, y);

                switch (symbol)
                {
                    case 'W': // Wall 
                        walls.Add(pos);
                        break;
                    case 'L': // Lava 
                        lava.Add(pos);
                        break;
                    case 'I': // Player 
                        player = new Player(pos);
                        break;
                    // Враги
                    case 'P': enemies.Add(new Enemy(pos, EnemyType.Pawn)); break;
                    case 'K': enemies.Add(new Enemy(pos, EnemyType.Knight)); break;
                    case 'B': enemies.Add(new Enemy(pos, EnemyType.Bishop)); break;
                    case 'R': enemies.Add(new Enemy(pos, EnemyType.Rook)); break;
                    case 'Q': enemies.Add(new Enemy(pos, EnemyType.Queen)); break;
                }
            }
        }

        return new Level("New Level", room, walls, lava, player, enemies);
    }
}