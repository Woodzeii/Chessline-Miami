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

    public void DrawAttackPreview(Graphics g, Player player, Point cameraOffset)
    {
        if (!player.IsAttacking) return;

        var cellSize = _constants.CellSize;
        var target = player.AttackTarget;

        // Проверка цель-диагональ?
        var dx = Math.Abs(target.X - player.FieldPos.X);
        var dy = Math.Abs(target.Y - player.FieldPos.Y);

        // Показывать атаки 
        if (dx == dy && dx ==1 )
        {
            var rect = new Rectangle(
                target.X * cellSize + cameraOffset.X,
                target.Y * cellSize + cameraOffset.Y,
                cellSize,
                cellSize
            );

            // Красное выделение цели
            using (var brush = new SolidBrush(Color.FromArgb(100, 255, 0, 0)))
            {
                g.FillRectangle(brush, rect);
            }

            // Граница
            using (var pen = new Pen(Color.Red, 3))
            {
                g.DrawRectangle(pen, rect);
            }
        }
    }
}