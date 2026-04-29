using ChessLine_Miami.Models;
using ChessLine_Miami.Presenters;
namespace ChessLine_Miami.UI;
public class EnemiesViewer
{
    public void DrawEnemies(Graphics g, List<Enemy> enemies, Point cameraOffset)
    {
        var cellSize = 40;
        foreach (var enemy in enemies)
        {
            var enemyRect = new Rectangle(enemy.Pos.X * cellSize + cameraOffset.X, enemy.Pos.Y * cellSize + cameraOffset.Y, cellSize, cellSize);
            g.FillRectangle(Brushes.Red, enemyRect);
        }
    }
}