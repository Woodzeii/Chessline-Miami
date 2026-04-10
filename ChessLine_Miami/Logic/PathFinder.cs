using ChessLine_Miami.Models;

namespace ChessLine_Miami.Logic;

public class PathFinder
{
    public void FindPath()
    {
    }

    public bool IsPlayerSeen(Player player, Enemy enemy, Level level)
    {
        //Алгоритм Брезенхема
        int x0 = enemy.Pos.X;
        int y0 = enemy.Pos.Y;
        int x1 = player.Pos.X;
        int y1 = player.Pos.Y;

        int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        int x = x0;
        int y = y0;

        while (true)
        {
            // Если текущая клетка (не начальная и не конечная) - стена, видимость заблокирована
            if ((x != x0 || y != y0) && (x != x1 || y != y1))
            {
                if (level.GetSector(x, y) == SectorType.Wall)
                    return false;
            }

            if (x == x1 && y == y1) break;

            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y += sy;
            }
        }
        return true;
    }
}