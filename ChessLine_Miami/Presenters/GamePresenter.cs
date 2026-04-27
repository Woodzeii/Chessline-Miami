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
        _game.Restart();
    }

    
}