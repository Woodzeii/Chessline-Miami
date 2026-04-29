using ChessLine_Miami.Models;
using ChessLine_Miami.Presenters;
namespace ChessLine_Miami.UI;
public class LevelViewer
{
    private const int CellSize = 40;
    
    public void DrawLevel(Graphics g, Level level, Point cameraOffset)
    {
        for (int x = 0; x < level.Size.Width; x++)
        {
            for (int y = 0; y < level.Size.Height; y++)
            {
                var cellRect = new Rectangle(
                    x * CellSize + cameraOffset.X, 
                    y * CellSize + cameraOffset.Y, 
                    CellSize, 
                    CellSize
                );
                
                var sector = level.GetSector(x, y);
                switch (sector)
                {
                    case SectorType.Wall:
                        g.FillRectangle(Brushes.Gray, cellRect);
                        break;
                    case SectorType.Lava:
                        g.FillRectangle(Brushes.OrangeRed, cellRect);
                        break;
                    default:
                        g.FillRectangle(Brushes.LightGray, cellRect);
                        break;
                }
            }
        }
    }
    
    public static int GetCellSize() => CellSize;
}