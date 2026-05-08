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

    public bool MoveOnKey(KeyEventArgs e, int deltaX, int deltaY, Keys key, int steps)
    {
        if (e.KeyCode == key)
        {
            _game.Player.TryMove(deltaX, deltaY, _game, steps);
            return true;
        }
        return false;
    }
    
    public bool WASD(KeyEventArgs e, bool isRush)
    {
        var moved = false;
        var steps = isRush ? 2 : 1;

        moved |= MoveOnKey(e, 0, -1, Keys.W, steps); // W - up
        moved |= MoveOnKey(e, 0, 1, Keys.S, steps);  // S - down
        moved |= MoveOnKey(e, -1, 0, Keys.A, steps);  // A - left
        moved |= MoveOnKey(e, 1, 0, Keys.D, steps);   // D - right
        return moved;
    }

    public void MovePlayer(int deltaX, int deltaY)
    {
        _game.Player.TryMove(deltaX, deltaY, _game);
    }

    

    
}