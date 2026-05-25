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

    public Player player => _game.Player;

    public bool MoveOnKey(KeyEventArgs e, int deltaX, int deltaY, Keys key, int steps)
    {
        if (e.KeyCode == key)
        {
            player.TryMove(deltaX, deltaY, _game, steps);
            return true;
        }
        return false;
    }
    
    public bool WASD(KeyEventArgs e, bool isRPressed)
    {

        bool isMovementKey = e.KeyCode == Keys.W || e.KeyCode == Keys.S || 
                     e.KeyCode == Keys.A || e.KeyCode == Keys.D;
                         
        if (!isMovementKey) return false;

        bool dynamicRushActive = isRPressed && player.IsRushReady();
        int steps = dynamicRushActive ? 2 : 1;


        var moved = false;
        moved |= MoveOnKey(e, 0, -1, Keys.W, steps); // W - up
        moved |= MoveOnKey(e, 0, 1, Keys.S, steps);  // S - down
        moved |= MoveOnKey(e, -1, 0, Keys.A, steps);  // A - left
        moved |= MoveOnKey(e, 1, 0, Keys.D, steps);   // D - right
        if (moved)
        {
            if (dynamicRushActive)
            {
                // Рывоr сделан
                player.RushCooldown = Player.StepsToRush; 
            }
            else
            {
                // Сделан обычный шаг, уменьшаем кулдаун на х..од
                player.RushCooldown = Math.Max(0, player.RushCooldown - 1);
            }
        }

        return moved;
    }

    public void MovePlayer(int deltaX, int deltaY)
    {
        player.TryMove(deltaX, deltaY, _game);
    }

    

    
}