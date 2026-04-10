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

    //TODO 
    public static bool IsDeadlyForPlayer(Point newPos,Level level)
    {
        throw new Exception("чё");
    }
    public static bool IsDedlyForEnemy(Point newPos, Level level)
    {
        throw new Exception("чё");
    }
}