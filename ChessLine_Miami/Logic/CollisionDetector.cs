namespace ChessLine_Miami.Logic;
using Models;
using Controllers;

public class CollisionDetector
{
    public static bool CanMoveTo(Point newPos, Level level)
    {
        var x = newPos.X;
        var y = newPos.Y;
        if (x >= 0 || y >= 0 || x < level.Size.Width || y < level.Size.Height) return false;
        if (level.Field[x,y]==SectorType.Wall) return  false;
        return true;
    }
    
    
/// <summary>
/// Проверяем столкновение игрока и врага на клетке
/// </summary>
/// <param name="player">игрок</param>
/// <param name="enemies">Список врагов</param>
/// <returns></returns>
    public static bool CheckCollision(Player player, List<Enemy> enemies)
    {
        return (enemies.Any(enemy => enemy.Pos == player.FieldPos));
    }


    // Проверяет, попал ли игрок по врагу
    public static List<Enemy> GetHitEnemies(Player player, Point mouseFieldPos, List<Enemy> enemies, Level level)
    {
        List<Enemy> hitEnemies = new List<Enemy>();


        int dx = 0;
        int dy = 0;
        if (mouseFieldPos.X > player.FieldPos.X) dx = 1;
        else if (mouseFieldPos.X < player.FieldPos.X) dx = -1;
        if (mouseFieldPos.Y > player.FieldPos.Y) dy = 1;
        else if (mouseFieldPos.Y < player.FieldPos.Y) dy = -1;
    
        if (dx == 0 && dy == 0) return hitEnemies;

        int checkX = player.FieldPos.X + dx;
        int checkY = player.FieldPos.Y + dy;
        
        if (checkX < 0 || checkX >= level.Size.Width || checkY < 0 || checkY >= level.Size.Height) return hitEnemies;
        
            
        
        foreach (var enemy in enemies)
        {
            if (enemy.Pos.X == checkX && enemy.Pos.Y == checkY)
            { hitEnemies.Add(enemy);
                break; 
            }
        }
        return hitEnemies;
    }
    

    

    
    
    
}