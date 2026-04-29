using System;
using ChessLine_Miami.Logic;
namespace ChessLine_Miami.Models;

public class Enemy
{
    public EnemyType Type;
    public Point Pos;
    public bool IsAlive;
    public bool IsPlayerSeen { get; set; }

    public Enemy(Point p, EnemyType type)
    {
        Pos = p;
        Type = type;
        IsAlive= true;
        IsPlayerSeen = false;
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

        // Враги не проходят сквозь врагов
        if (game.Enemies.Any(e => e.Pos.X == newX && e.Pos.Y == newY))
            return; 

        // Обновляем позицию врага
        Pos = new Point(newX, newY);
    }

    public void Kill()
    {
        IsAlive = false;
    }

    

     public void Move(Player player, Level level)
    {
        switch (Type)
        {
            case EnemyType.Rook:
                // Логика движения ладьи
                break;
            case EnemyType.Bishop:
                // Логика движения слона
                break;
            case EnemyType.Queen:
                // Логика движения ферзя (комбинация ладьи и слона)
                break;
            case EnemyType.Knight:
                // Логика движения коня
                break;
            case EnemyType.Pawn:
                // Логика движения пешки 
                break;
        }
    }
}