using System.IO;
using System.Media;
using System.Windows.Media;
using ChessLine_Miami.UI;
using ChessLine_Miami.Presenters;
using ChessLine_Miami.Models;
using ChessLine_Miami.Logic;
namespace ChessLine_Miami;

static class Program
{

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    /// 
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        
        var musicPath =  Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI/Music/Ambient.wav"); 
        // ^ Даже если расширение .wav, внутри может быть mp3, MediaPlayer это проглотит.

        var mediaPlayer = new MediaPlayer();
        mediaPlayer.Open(new Uri(musicPath));
        mediaPlayer.Play();
        // Создаем форму
        var form = new GameForm();

        // Создаем модель (игру)
        var lvl = LevelGenerator.LoadFromStringArray(Levels.level2.Split('\n').Where(s => s.Length > 0).ToArray());
        var game = new Game(lvl); 

        // Создаем презентер и связываем его с формой и моделью
        var presenter = new GamePresenter(game, form);

        form.SetPresenter(presenter);
        form.SetGame(game);

        Application.Run(form);
    }
}