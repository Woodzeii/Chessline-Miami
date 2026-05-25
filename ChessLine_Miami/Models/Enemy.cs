using System.Drawing;
using ChessLine_Miami.Logic;

namespace ChessLine_Miami.Models;

public class Enemy
{
    public EnemyType Type;
    public Point Pos;
    public PointF RenderFieldPos;
    public bool IsAlive;
    public bool IsPlayerSeen { get; set; }

    public Enemy(Point p, EnemyType type)
    {
        Pos = p;
        Type = type;
        IsAlive = true;
        IsPlayerSeen = false;
        ResetRenderPosition();
    }

    public void ResetRenderPosition()
    {
        RenderFieldPos = new PointF(Pos.X, Pos.Y);
    }

    public void UpdateRenderPosition(float smoothing = Player.FrameUpdateSpeed)
    {
        var target = new PointF(Pos.X, Pos.Y);
        RenderFieldPos = new PointF(
            RenderFieldPos.X + (target.X - RenderFieldPos.X) * smoothing,
            RenderFieldPos.Y + (target.Y - RenderFieldPos.Y) * smoothing
        );

        if (Math.Abs(target.X - RenderFieldPos.X) < 0.01f && Math.Abs(target.Y - RenderFieldPos.Y) < 0.01f)
        {
            RenderFieldPos = target;
        }
    }

    public void TryMove(int deltaX, int deltaY, Game game)
    {
        if (!IsAlive) return;

        int newX = Pos.X + deltaX;
        int newY = Pos.Y + deltaY;

        // Проверяем границы уровня
        if (newX < 0 || newX >= game.Level.Size.Width || newY < 0 || newY >= game.Level.Size.Height)
            return;

        // Проверяем на стену
        if (game.Level.Field[newX, newY] == SectorType.Wall)
            return;

        // Проверяем на лаву
        if (game.Level.Field[newX, newY] == SectorType.Lava)
        {
            IsAlive = false;
            return;
        }

        // Враги не проходят сквозь живых врагов
        if (game.Enemies.Any(e => e.IsAlive && e.Pos.X == newX && e.Pos.Y == newY))
            return;

        // Обновляем позицию врага
        Pos = new Point(newX, newY);
    }

    public void Kill()
    {
        IsAlive = false;
    }
}