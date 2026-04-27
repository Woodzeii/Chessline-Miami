using ChessLine_Miami.Models;

namespace ChessLine_Miami.Presenters;

public class GamePresenter
{
    private readonly Game _game;

    public GamePresenter(Game game)
    {
        _game = game;
    }

    public void StartNewGame()
    {
        // Инициализация новой игры
    }

    public void PauseGame()
    {
        _game.IsPaused = true;
    }

    public void ResumeGame()
    {
        _game.IsPaused = false;
    }

    public void EndGame()
    {
        // Завершение игры
    }
}