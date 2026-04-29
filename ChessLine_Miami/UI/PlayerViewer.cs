using System.IO;
using ChessLine_Miami.Presenters;
using ChessLine_Miami.Models;
using System.Drawing;
namespace ChessLine_Miami.UI;


public class PlayerViewer
{
    Image playerImg = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI/Photo/pawn.png")); 
    public void DrawPlayer(Graphics g, Player player, Point cameraOffset)
    {
        var cellSize = _constants.CellSize;
        var playerRect = new Rectangle(
            player.FieldPos.X * cellSize + cameraOffset.X, 
            player.FieldPos.Y * cellSize + cameraOffset.Y, 
            cellSize, 
            cellSize
        );
        g.DrawImage(playerImg, playerRect);
    }

}