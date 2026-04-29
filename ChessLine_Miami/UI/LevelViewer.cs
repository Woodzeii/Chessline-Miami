using ChessLine_Miami.Models;
using ChessLine_Miami.Presenters;
namespace ChessLine_Miami.UI;
public class LevelViewer
{
    public void DrawLevel(Graphics g, Level level, Point cameraOffset)
    {
        var cellSize = 40;
        for (int x = 0; x < level.Size.Width; x++)
        {
            for (int y = 0; y < level.Size.Height; y++)
            {
                var cellRect = new Rectangle(x * cellSize + cameraOffset.X, y * cellSize + cameraOffset.Y, cellSize, cellSize);
                if (level.Walls.Any(wall => wall.X == x && wall.Y == y))
                {
                    g.FillRectangle(Brushes.Gray, cellRect);
                }
                else if (level.Lava.Any(l => l.X == x && l.Y == y))
                {
                    g.FillRectangle(Brushes.OrangeRed, cellRect);
                }
                else
                {
                    g.FillRectangle(Brushes.LightGray, cellRect);
                }
            }
        }
    }
}