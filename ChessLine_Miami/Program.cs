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

    private static readonly List<Level> levels = Levels.AllLevels
    .Select(lvlStr => LevelGenerator
    .LoadFromStringArray(lvlStr.Split('\n').Where(s => s.Length > 0).ToArray()))
    .ToList();



    static GameForm Form;
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    /// 
    /// 
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        
        var musicPath =  Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI/Music/Ambient.wav"); 

        var mediaPlayer = new MediaPlayer();
        mediaPlayer.Open(new Uri(musicPath));
        mediaPlayer.Play();
        // Создаем форму
        Form = new GameForm();

        // Создаем модель (игру) и презентер, связывая их с формой
        LoadNextLevel();

        Application.Run(Form);
    }

    
    public static void LoadNextLevel()
    {
        if (currentLevelIndex >= levels.Count)
        {
            MessageBox.Show("Игра пройдена!");
            Application.Exit();
            return;
        }
        var lvlData = levels[currentLevelIndex];
        // 2. Создаем новую игру
        var game = new Game(lvlData);
        var presenter = new GamePresenter(game, Form);

        game.OnFinished += () => {
            currentLevelIndex++;
            LoadNextLevel(); 
        };
    
        Form.SetPresenter(presenter);
        Form.SetGame(game);
    }
}