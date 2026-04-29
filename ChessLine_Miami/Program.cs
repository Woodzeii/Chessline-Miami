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
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // 1. Создаем форму
        var form = new GameForm();

        // 2. Создаем модель (игру)
        var lvl = LevelGenerator.LoadFromStringArray(Levels.Level1.Split('\n').Where(s => s.Length > 0).ToArray());
        var game = new Game(lvl); 

        // 3. Создаем презентер и связываем его с формой и моделью
        var presenter = new GamePresenter(game, form);

        // 4. ВОТ ЗДЕСЬ вызываем SetPresenter
        form.SetPresenter(presenter);
        form.SetGame(game); // Игру тоже нужно передать

        Application.Run(form);
    }
}