using ChessLine_Miami.Models;
using ChessLine_Miami.UI;
using ChessLine_Miami.Logic;
using System.Drawing;
using System.IO;
using System.Windows.Media;

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

    public void RestartLevel()
    {
        _game.Restart();
        _view.Redraw();
    }

    public async Task OnKeyDown(KeyEventArgs e, bool isRush)
    {
        if (await EndIfDead()) return;
        if (_game.IsLevelFinished())
        {
            MessageBox.Show("Level completed! Starting next level.");
            await Task.Delay(500);
            _game.FinishLevel();
            return;
        }
        var moved = _playerPresenter.WASD(e, isRush);
        
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

    

    public PointF GetCameraOffsetF(Size screenSize)
    {
        var cellSize = _constants.CellSize;
        var playerPixelPosX = Player.RenderFieldPos.X * cellSize;
        var playerPixelPosY = Player.RenderFieldPos.Y * cellSize;
        var offsetX = screenSize.Width / 2f - playerPixelPosX - cellSize / 2f;
        var offsetY = screenSize.Height / 2f - playerPixelPosY - cellSize / 2f;
        return new PointF(offsetX, offsetY);
    }

    public void UpdateAnimations()
    {
        _game.Player.UpdateRenderPosition();
        foreach (var enemy in _game.Enemies)
        {
            enemy.UpdateRenderPosition();
        }
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
        if (dx == dy && dx == 1)
        {
            foreach (var enemy in _game.Enemies.Where(e=>e.Pos == player.AttackTarget))
            {
                enemy.Kill();

                var musicPath =  Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI/SFX/hotline-miami-hit.wav"); 

                        var mediaPlayer = new MediaPlayer();
                        mediaPlayer.Open(new Uri(musicPath));
                        mediaPlayer.Play();


             
            }
            EndIfDead(); // Проверяем, не умер ли игрок от контратаки
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