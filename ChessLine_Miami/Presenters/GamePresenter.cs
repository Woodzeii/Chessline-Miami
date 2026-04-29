using ChessLine_Miami.Models;
using ChessLine_Miami.UI;
using ChessLine_Miami.Logic;
using System;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
namespace ChessLine_Miami.Presenters;

public class GamePresenter
{
    private LevelPresenter _levelPresenter;
    private EnemiesPresenter _enemiesPresenter;
    private PlayerPresenter _playerPresenter;
    private IGameView _view;
    private readonly Game _game;
    

    public GamePresenter(Game game, IGameView view)
    {
        _game = game;
        _view = view;
        _playerPresenter = new PlayerPresenter(game);
        _levelPresenter = new LevelPresenter(game.Level);
        _enemiesPresenter = new EnemiesPresenter(game);
    }

    public void StartNewGame()
    {
        _game.Restart();
        _view.SetGame(_game);
        
    }

    public async Task OnKeyDown(KeyEventArgs e)
    {
        if (await EndIfDead()) return;
        var moved = _playerPresenter.WASD(e);
        
        if (moved)
        {
            _view.Redraw();
            await Task.Delay(300);
            System.Diagnostics.Debug.WriteLine("=== Updating enemies ===");
            _enemiesPresenter.UpdateEnemies();
            System.Diagnostics.Debug.WriteLine($"=== Enemies after update: {_game.Enemies.Count} ===");
            _view.Redraw();
            if (await EndIfDead()) return;
        }
    }

    private async Task<bool> EndIfDead()
    {
        _game.Player.IsAlive = IsPlayerAlive(_game.Player, _game.Enemies, _game.Level.Field);
        if (!_game.Player.IsAlive)        {
            MessageBox.Show("You died! Starting new game.");
            await Task.Delay(500);
            StartNewGame();
            return true;
        }

        return false;
    }

    public bool IsPlayerAlive(Player player, List<Enemy> enemies, SectorType[,] field)
    {
        if (field[player.FieldPos.X, player.FieldPos.Y] == SectorType.Lava)
            return false;
        if (CollisionDetector.CheckCollision(player, enemies))
            return false;
        return true;
    }

    public Point GetCameraOffset(Size screenSize)
    {
        var cellSize = _constants.CellSize;
        var playerPixelPos = new Point(Player.FieldPos.X * cellSize, Player.FieldPos.Y * cellSize);
        var offsetX = screenSize.Width / 2 - playerPixelPos.X - cellSize / 2;
        var offsetY = screenSize.Height / 2 - playerPixelPos.Y - cellSize / 2;
        return new Point(offsetX, offsetY);
    }

    public void UpdateAttackPreview(int mouseX, int mouseY)
    {
        var player = _game.Player;
        var dx = Math.Abs(mouseX - player.FieldPos.X);
        var dy = Math.Abs(mouseY - player.FieldPos.Y);

        // Check if target is on a diagonal (4 diagonal directions)
        if (dx == dy && dx > 0)
        {
            player.SetAttackTarget(new Point(mouseX, mouseY));
            _view.Redraw();
        }
        else
        {
            player.ClearAttack();
            _view.Redraw();
        }
    }

    public async Task ExecuteAttack(int targetX, int targetY)
    {
        var player = _game.Player;
        var dx = Math.Abs(targetX - player.FieldPos.X);
        var dy = Math.Abs(targetY - player.FieldPos.Y);

        // Only allow diagonal attacks
        if (dx == dy && dx > 0)
        {
            foreach (var enemy in _game.Enemies.Where(e=>e.Pos == player.AttackTarget))
            {
                enemy.Kill();
            }
            
            System.Diagnostics.Debug.WriteLine($"Attack at ({targetX}, {targetY})");
            player.ClearAttack();
            await Task.Delay(300);
            System.Diagnostics.Debug.WriteLine("=== Updating enemies ===");
            _enemiesPresenter.UpdateEnemies();
            System.Diagnostics.Debug.WriteLine($"=== Enemies after update: {_game.Enemies.Count} ===");
            _view.Redraw();
        }
    }

    public void ClearAttack()
    {
        _game.Player.ClearAttack();
        _view.Redraw();
    }
    
    public Player Player => _game.Player;
    public Level Level => _game.Level;
    public List<Enemy> Enemies => _game.Enemies;
}