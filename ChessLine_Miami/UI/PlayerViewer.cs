using ChessLine_Miami.Presenters;
using ChessLine_Miami.Models;
using System.Drawing;
namespace ChessLine_Miami.UI;


public class PlayerViewer
{
    public void DrawPlayer(Graphics g, Player player)
    {
        var cellSize = 40;
        var playerRect = new Rectangle(player.FieldPos.X * cellSize, player.FieldPos.Y * cellSize, cellSize, cellSize);
        g.FillEllipse(Brushes.Blue, playerRect);
    }

}