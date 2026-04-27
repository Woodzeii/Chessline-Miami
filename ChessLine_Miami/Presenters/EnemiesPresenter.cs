using ChessLine_Miami.Models;
using ChessLine_Miami.Logic;

namespace ChessLine_Miami.Presenters;

public class EnemiesPresenter
{
    private readonly Game _game;

    public EnemiesPresenter(Game game)
    {
        _game = game;
    }

    public List<Enemy> Enemies => _game.Enemies;

    public void UpdateEnemies()
    {
        foreach (var enemy in _game.Enemies.Where(e => e.IsAlive))
        {
            // Логика обновления врагов
        }
    }

    public void MoveEnemy(Enemy enemy, int deltaX, int deltaY)
    {
        if (enemy.IsAlive)
        {
            enemy.TryMove(deltaX, deltaY, _game.Level);
        }
    }

    public List<Enemy> GetActiveEnemies()
    {
        return _game.Enemies.Where(e => e.IsAlive).ToList();
    }
}