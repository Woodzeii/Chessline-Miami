using ChessLine_Miami.Models;
using ChessLine_Miami.Logic;

namespace ChessLine_Miami.Controllers;

public class LevelController
{
    //todo сделать г
    public static void LevelRestart(Level level)
    {
        level = LevelGenerator.LoadFromStringArray(level.Maplines);
    }
        
    public static bool IsPlayerAlive(Player player,List<Enemy> enemies, SectorType[,] field)
    {
        if  (field[player.FieldPos.X, player.FieldPos.Y]==SectorType.Lava) return false;
        if (CollisionDetector.CheckCollision(player,enemies)) return false;
        return true;
    }
    public static bool IsEnemyDead(List<Enemy> enemies, SectorType[,] field)
    {
        enemies
            .Where(enemy=>field[enemy.Pos.X,enemy.Pos.Y]==SectorType.Lava)
            .Select(x=>x.IsAlive==false);
        //todo добаввить проверку на удар
        return true;
    }
    
    public Point ScreenToField(Point mouseScreenPos, Point cameraOffset, int cellSize)
    {
        int worldX = mouseScreenPos.X + cameraOffset.X;
        int worldY = mouseScreenPos.Y + cameraOffset.Y;
    
        int fieldX = worldX / cellSize;
        int fieldY = worldY / cellSize;
    
        return new Point(fieldX, fieldY);
    }

    public Point GetDirectionFromPlayerToTarget(Point playerFieldPos, Point targetFieldPos)
    {
        int dx = 0;
        int dy = 0;
    
        if (targetFieldPos.X > playerFieldPos.X)
            dx = 1;
        else if (targetFieldPos.X < playerFieldPos.X)
            dx = -1;
    
        if (targetFieldPos.Y > playerFieldPos.Y)
            dy = 1;
        else if (targetFieldPos.Y < playerFieldPos.Y)
            dy = -1;
    
        return new Point(dx, dy);
    }
    
    
}