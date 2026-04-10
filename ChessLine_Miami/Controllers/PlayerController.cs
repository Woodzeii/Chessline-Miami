using ChessLine_Miami.Models;
using ChessLine_Miami.Logic;

namespace ChessLine_Miami.Controllers;

public class PlayerController
{
    public void MoveOnButton(Level level, KeyEventArgs e,int deltaX,int deltaY, Keys key)
    {
        if (e.KeyCode == key)
        {
            level.Player.TryMove(deltaX, deltaY,level);
        }
    }
}