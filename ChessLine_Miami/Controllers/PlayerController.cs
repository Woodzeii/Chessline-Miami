using ChessLine_Miami.Models;
using ChessLine_Miami.Logic;

namespace ChessLine_Miami.Controllers;

public class PlayerController
{
    public void MoveOnButton(Level level, KeyEventArgs e,int deltaX,int deltaY, Keys key)
    {
        if (e.KeyCode == key)
        {
            var newPos = new Point(level.Player.Pos.X+deltaX, level.Player.Pos.Y+deltaY);
            if (CollisionDetector.CanMoveTo(newPos, level))
            {
             if (CollisionDetector.IsDeadlyForPlayer(newPos,level)) Controllers.LevelRestart();
             else LevelController.MoveTo(SectorType.Player, level.Player.Pos,newPos,level);
            }


        }
    }
}