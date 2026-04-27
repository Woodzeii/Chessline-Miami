using ChessLine_Miami.Models;
using System.Windows.Forms;

namespace ChessLine_Miami.Presenters;

public class PlayerPresenter
{
    private readonly Game _game;

    public PlayerPresenter(Game game)
    {
        _game = game;
    }

    public Player Player => _game.Player;

    public void MoveOnKey(KeyEventArgs e, int deltaX, int deltaY, Keys key)
    {
        if (e.KeyCode == key)
        {
            _game.Player.TryMove(deltaX, deltaY, _game.Level);
        }
    }

    public void MovePlayer(int deltaX, int deltaY)
    {
        _game.Player.TryMove(deltaX, deltaY, _game.Level);
    }
}