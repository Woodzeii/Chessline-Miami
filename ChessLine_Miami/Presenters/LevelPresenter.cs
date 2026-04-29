using ChessLine_Miami.Models;
using ChessLine_Miami.Logic;
using System.Windows.Forms;

namespace ChessLine_Miami.Presenters;

public class LevelPresenter
{
    private readonly Level _level;

    public LevelPresenter(Level level)
    {
        _level = level;
    }

    public Level Level => _level;
    public Point PlayerSpawn => _level.PlayerSpawn;
    public List<Enemy> EnemySpawns => _level.EnemySpawns;

    public void Restart(Game game)
    {
        game.Restart();
    }

    public bool IsPlayerAlive(Player player, List<Enemy> enemies, SectorType[,] field)
    {
        if (field[player.FieldPos.X, player.FieldPos.Y] == SectorType.Lava)
            return false;
        if (CollisionDetector.CheckCollision(player, enemies))
            return false;
        return true;
    }

    public bool AreEnemiesDead(List<Enemy> enemies, SectorType[,] field)
    {
        return enemies
            .Where(enemy => field[enemy.Pos.X, enemy.Pos.Y] == SectorType.Lava)
            .All(x => x.IsAlive == false);
    }

    public Point ScreenToField(Point mouseScreenPos, Point cameraOffset, int cellSize)
    {
        int worldX = mouseScreenPos.X + cameraOffset.X;
        int worldY = mouseScreenPos.Y + cameraOffset.Y;

        int fieldX = worldX / cellSize;
        int fieldY = worldY / cellSize;

        return new Point(fieldX, fieldY);
    }

   
}