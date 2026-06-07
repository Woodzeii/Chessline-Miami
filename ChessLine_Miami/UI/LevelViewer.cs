using System.IO;
using ChessLine_Miami.Models;
using ChessLine_Miami.Presenters;
using ChessLine_Miami;
namespace ChessLine_Miami.UI;
public class LevelViewer
{
    private int CellSize = _constants.CellSize;
    private static Image? lavaImg = null;
    private const double NeonFlashDurationSeconds = 0.250;
    
    public void DrawLevel(Graphics g, Game game, Point cameraOffset)
    {
        var level = game.Level;
        var neonActive = game.Stats.TotalKills > 0 && (DateTime.Now - game.LastKillTime).TotalSeconds < NeonFlashDurationSeconds;
        var neonStrength = neonActive
            ? 1f - (float)((DateTime.Now - game.LastKillTime).TotalSeconds / NeonFlashDurationSeconds)
            : 0f;
        var neonColor = Color.FromArgb(
            (int)(150 * neonStrength),
            180,
            40,
            255);
        var wallBrushColor = neonActive
            ? Color.FromArgb(80, 40, 10, 80)
            : Color.FromArgb(5, 5, 10);
        var wallHighlightColor = neonActive
            ? Color.FromArgb(220, 140, 40, 255)
            : Color.White;
        var glowColor = neonActive
            ? Color.FromArgb((int)(180 * neonStrength), 180, 0, 255)
            : Color.FromArgb(100, 150, 255);

        // Загружаем изображение лавы лениво только если пользователь не включил простой режим
        if (!Program.PlayerProgress.UseSimpleLavaTiles && lavaImg == null)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI/Photo/Lava.jpg");
            if (File.Exists(path))
            {
                try { lavaImg = Image.FromFile(path); } catch { lavaImg = null; }
            }
        }
        using var wallBrush = new SolidBrush(wallBrushColor);
        using var wallHighlightPen = new Pen(wallHighlightColor, 4);
        using var glowPen = new Pen(glowColor, 6);

        bool IsWall(int px, int py)
            => px >= 0 && py >= 0 && px < level.Size.Width && py < level.Size.Height && level.GetSector(px, py) == SectorType.Wall;

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
                        g.FillRectangle(wallBrush, cellRect);

                        if (!IsWall(x, y - 1))
                        {
                            g.DrawLine(glowPen, cellRect.Left, cellRect.Top, cellRect.Right, cellRect.Top);
                            g.DrawLine(wallHighlightPen, cellRect.Left, cellRect.Top, cellRect.Right, cellRect.Top);
                        }
                        if (!IsWall(x, y + 1))
                        {
                            g.DrawLine(glowPen, cellRect.Left, cellRect.Bottom - 1, cellRect.Right, cellRect.Bottom - 1);
                            g.DrawLine(wallHighlightPen, cellRect.Left, cellRect.Bottom - 1, cellRect.Right, cellRect.Bottom - 1);
                        }
                        if (!IsWall(x - 1, y))
                        {
                            g.DrawLine(glowPen, cellRect.Left, cellRect.Top, cellRect.Left, cellRect.Bottom);
                            g.DrawLine(wallHighlightPen, cellRect.Left, cellRect.Top, cellRect.Left, cellRect.Bottom);
                        }
                        if (!IsWall(x + 1, y))
                        {
                            g.DrawLine(glowPen, cellRect.Right - 1, cellRect.Top, cellRect.Right - 1, cellRect.Bottom);
                            g.DrawLine(wallHighlightPen, cellRect.Right - 1, cellRect.Top, cellRect.Right - 1, cellRect.Bottom);
                        }
                        break;
                    case SectorType.Lava:
                        if (Program.PlayerProgress.UseSimpleLavaTiles)
                        {
                            using var lavaBrush = new SolidBrush(Color.Orange);
                            g.FillRectangle(lavaBrush, cellRect);
                        }
                        else if (lavaImg != null)
                        {
                            g.DrawImage(lavaImg, cellRect);
                        }
                        break;
                    default:
                    {
                        var floorColor = ((x + y) % 2 == 0)
                            ? Color.FromArgb(24, 16, 42)
                            : Color.FromArgb(14, 8, 24);
                        using var floorBrush = new SolidBrush(floorColor);
                        g.FillRectangle(floorBrush, cellRect);
                        break;
                    }
                }
            }
        }
    }
}