using System.IO;
using ChessLine_Miami.Models;
using ChessLine_Miami.Presenters;
using Microsoft.VisualBasic;
namespace ChessLine_Miami.UI;
public class EnemiesViewer
{
    Image bishopImg = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI/Photo/bishop.png"));
    Image knightImg = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI/Photo/knight.png"));
    Image rookImg = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI/Photo/rook.png"));
    Image pawnImg = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI/Photo/pawnEn.png"));
    Image kingImg = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI/Photo/king.png"));
    Image queenImg = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI/Photo/queen.png"));
    Image bloodImg = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI/Photo/blood.png"));
    public void DrawEnemies(Graphics g, List<Enemy> enemies, Point cameraOffset)
    {
        var cellSize = _constants.CellSize;
        foreach (var enemy in enemies)
        {
            var enemyRect = new Rectangle(enemy.Pos.X * cellSize + cameraOffset.X, enemy.Pos.Y * cellSize + cameraOffset.Y, cellSize, cellSize);
            if (!enemy.IsAlive)
            {
                g.DrawImage(bloodImg, enemyRect);
                continue;
            }
            switch (enemy.Type)
            {
                case EnemyType.Rook:
                    g.DrawImage(rookImg, enemyRect);
                    break;
                case EnemyType.Knight:
                    g.DrawImage(knightImg, enemyRect);
                    break;
                case EnemyType.Bishop:
                    g.DrawImage(bishopImg, enemyRect);
                    break;
                case EnemyType.Queen:
                    g.DrawImage(queenImg, enemyRect);
                    break;
                case EnemyType.Pawn:
                    g.DrawImage(pawnImg, enemyRect);
                    break;
            }
        }
    }
}