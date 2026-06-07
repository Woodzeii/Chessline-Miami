using System.IO;
using System.Media;
using System.Windows.Media;
using ChessLine_Miami.UI;
using ChessLine_Miami.Presenters;
using ChessLine_Miami.Models;
using ChessLine_Miami.Logic;
using Microsoft.VisualBasic;
namespace ChessLine_Miami;

static class Program
{
    private static int currentLevelIndex = 0;

    private static readonly List<Level> levels = new List<Level>
    {
        LevelGenerator.LoadFromStringArray(Levels.AllLevels[0].Split('\n').Where(s => s.Length > 0).ToArray(), "StartLevel"),
        LevelGenerator.LoadFromStringArray(Levels.AllLevels[1].Split('\n').Where(s => s.Length > 0).ToArray(), "Level2"),
        LevelGenerator.LoadFromStringArray(Levels.AllLevels[2].Split('\n').Where(s => s.Length > 0).ToArray(), "Level3"),
        LevelGenerator.LoadFromStringArray(Levels.AllLevels[3].Split('\n').Where(s => s.Length > 0).ToArray(), "Level4"),
        LevelGenerator.LoadFromStringArray(Levels.AllLevels[4].Split('\n').Where(s => s.Length > 0).ToArray(), "Level5"),
        LevelGenerator.LoadFromStringArray(Levels.AllLevels[5].Split('\n').Where(s => s.Length > 0).ToArray(), "level6"),
        LevelGenerator.LoadFromStringArray(Levels.AllLevels[6].Split('\n').Where(s => s.Length > 0).ToArray(), "Level7"),
        LevelGenerator.LoadFromStringArray(Levels.AllLevels[7].Split('\n').Where(s => s.Length > 0).ToArray(), "Level8"),
        LevelGenerator.LoadFromStringArray(Levels.AllLevels[8].Split('\n').Where(s => s.Length > 0).ToArray(), "Level9"),
        LevelGenerator.LoadFromStringArray(Levels.AllLevels[9].Split('\n').Where(s => s.Length > 0).ToArray(), "Level10"),
    };

    static GameForm Form;
    public static PlayerProgress PlayerProgress { get; set; }
    private static MediaPlayer mediaPlayer;

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        
        // Загружаем прогресс игрока
        PlayerProgress = PlayerProgress.LoadProgress();
        // Инициализируем прогресс с пустыми именами уровней — пользователь задаст их отдельно
        PlayerProgress.InitializeLevels(new List<string> { "Basis", "Streets", "Warehouse", "Jailed", "Beasts", "Mongolians", "Castle of Lava", "Zergs!", "Stairway to hell", "Kings Palace" });
        SFX.PlayerProgress = PlayerProgress;
        
        // Стартовая музыка главного меню
        SFX.PlayMenuMusic();
        
        // Создаем форму
        Form = new GameForm();

        Application.Run(Form);
    }

    public static void UpdateVolume()
    {
        if (mediaPlayer != null)
        {
            mediaPlayer.Volume = PlayerProgress.Volume;
        }
    }

    public static void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Count)
            return;

        // Проверяем, может ли игрок играть этот уровень
        if (!PlayerProgress.CanPlayLevel(levelIndex))
        {
            MessageBox.Show("Вы не можете играть этот уровень. Пройдите предыдущие уровни первыми.", "Недоступен", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        currentLevelIndex = levelIndex;
        var lvlData = levels[currentLevelIndex];
        
        // Создаем новую игру
        var game = new Game(lvlData);
        var presenter = new GamePresenter(game, Form);

        game.OnFinished += () => {
            // Сохраняем результат уровня
            PlayerProgress.Levels[currentLevelIndex].UpdateProgress(game.Stats);
            PlayerProgress.SaveProgress();
            
            // Переходим на следующий уровень
            LoadNextLevel(); 
        };
    
        Form.SetPresenter(presenter);
        Form.SetGame(game);
        switch (currentLevelIndex)
        {
            case 0:
                SFX.PlayNightCrawler();
                break;
            case 1:
                SFX.PlayNormal();
                break;
            case 2:
                SFX.PlayNightCrawler();
                break;
            case 3:
                SFX.PlayEpic();
                break;
            case 4:
                SFX.PlayNormal();
                break;
            case 5:
                SFX.PlayNightCrawler();
                break;
            case 6:
                SFX.PlayHydrogen();
                break;
            case 7:
                SFX.PlayNightCrawler();
                break;
            case 8:
                SFX.Play1st();
                break;
            case 9:
                SFX.PlayActive();
                break;
        }
        
    }

    public static void LoadNextLevel()
    {
        int nextLevelIndex = PlayerProgress.GetNextUncompletedLevelIndex();
        if (PlayerProgress.Levels[nextLevelIndex].IsCompleted)
        {
            // Все уровни пройдены
            Form.ShowMainMenu();
            return;
        }
        
        LoadLevel(nextLevelIndex);
    }
}