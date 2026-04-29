using System.Drawing;

namespace ChessLine_Miami.Models;

public class Game
{
    public Level Level { get; set; }
    public Player Player { get; set; }
    public List<Enemy> Enemies { get; set; }
    public bool IsPaused { get; set; }
    public int Score { get; set; }

    public Game(Level level)
    {
        Level = level;
        Player = new Player(level.PlayerSpawn);
        Enemies = level.EnemySpawns
            .Select((enemy) => new Enemy(enemy.Pos, enemy.Type))
            .ToList();
        IsPaused = false;
        Score = 0;
    }
    
    public void Restart()
    {
        Player = new Player(Level.PlayerSpawn);
        Enemies = Level.EnemySpawns
            .Select((enemy) => new Enemy(enemy.Pos, enemy.Type))
            .ToList();
    }
}