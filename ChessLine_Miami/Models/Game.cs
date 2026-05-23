using System.Drawing;

namespace ChessLine_Miami.Models;

public class Game
{
    public Level Level { get; set; }
    public Player Player { get; set; }
    public List<Enemy> Enemies { get; set; }
    public bool IsPaused { get; set; }
    public event Action OnFinished;

    public int Score { get; set; }
    
    // Статистика уровня
    public DateTime LevelStartTime { get; set; }
    public LevelStats Stats { get; set; }
    public int ComboCount { get; set; }
    public DateTime LastKillTime { get; set; }
    public const int ComboTimeLimit = 10; // 10 секунд для комбо

    public void FinishLevel()
    {
        OnFinished?.Invoke();
    }

    public Game(Level level)
    {
        Level = level;
        Player = new Player(level.PlayerSpawn);
        Enemies = level.EnemySpawns
            .Select((enemy) => new Enemy(enemy.Pos, enemy.Type))
            .ToList();
        IsPaused = false;
        Score = 0;
        
        // Инициализируем статистику
        LevelStartTime = DateTime.Now;
        Stats = new LevelStats();
        ComboCount = 0;
        LastKillTime = DateTime.Now;
    }
    
    public void Restart()
    {
        Player = new Player(Level.PlayerSpawn);
        Enemies = Level.EnemySpawns
            .Select((enemy) => new Enemy(enemy.Pos, enemy.Type))
            .ToList();
    }
    public void LoadLevel()
    {
        // Сбросим статистику
        LevelStartTime = DateTime.Now;
        Stats = new LevelStats();
        ComboCount = 0;
        LastKillTime = DateTime.Now;
        Restart();
    }

    public bool IsLevelFinished()
    {
        if (Enemies.Where(e=> e.IsAlive).Count() == 0)
        {
            return true;
        }
        return false;
    }
    
    public void RegisterKill()
    {
        var timeSinceLastKill = (DateTime.Now - LastKillTime).TotalSeconds;
        
        // Если килл в пределах 10 секунд - продлеваем комбо
        if (timeSinceLastKill < ComboTimeLimit)
        {
            ComboCount++;
        }
        else
        {
            ComboCount = 1; // Начинаем новое комбо
        }
        
        LastKillTime = DateTime.Now;
        Stats.TotalKills++;
        Stats.ComboKills = Math.Max(Stats.ComboKills, ComboCount);
    }
    
    public void FinalizeStats()
    {
        Stats.TimeSeconds = (int)(DateTime.Now - LevelStartTime).TotalSeconds;
        Stats.CalculateRating();
    }
}