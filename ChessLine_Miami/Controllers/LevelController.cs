using ChessLine_Miami.Models;
using ChessLine_Miami.Logic;

namespace ChessLine_Miami.Controllers;

public class LevelController
{
    public static void MoveTo(SectorType subject,Point current, Point newPos, Level level)
    {
        //if (CollisionDetector.CanMoveTo(newPos, level))
        {
        level.Field[current.X, current.Y] = SectorType.Empty;
        level.Field[newPos.X, newPos.Y] = subject;
        }
    }
    //todo
    public static void LevelRestart(Level level)
    {
        level.CreateField();
    }
        

    
}