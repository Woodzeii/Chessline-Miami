using ChessLine_Miami.Models;
using ChessLine_Miami.UI;
using ChessLine_Miami.Logic;
using System;
using System.Windows.Forms;
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
        var moved = _playerPresenter.WASD(e);
        if (moved)
        {
            _view.Redraw();
            await Task.Delay(300);
            System.Diagnostics.Debug.WriteLine("=== Updating enemies ===");
            _enemiesPresenter.UpdateEnemies();
            System.Diagnostics.Debug.WriteLine($"=== Enemies after update: {_game.Enemies.Count} ===");
            _view.Redraw();
        }
    }

    public Point GetCameraOffset(Size screenSize)
    {
        var cellSize = 40;
        var playerPixelPos = new Point(Player.FieldPos.X * cellSize, Player.FieldPos.Y * cellSize);
        var offsetX = screenSize.Width / 2 - playerPixelPos.X - cellSize / 2;
        var offsetY = screenSize.Height / 2 - playerPixelPos.Y - cellSize / 2;
        return new Point(offsetX, offsetY);
    }
    
    public Player Player => _game.Player;
    public Level Level => _game.Level;
    public List<Enemy> Enemies => _game.Enemies;
}