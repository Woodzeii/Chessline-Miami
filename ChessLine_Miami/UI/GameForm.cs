using ChessLine_Miami.Models;
using ChessLine_Miami.Presenters;
using System;
using System.Windows.Forms;
namespace ChessLine_Miami.UI;
class _constants{
    public const int CellSize = 100;
}

public partial class GameForm : Form, IGameView
{
    public PlayerViewer PlayerViewer { get; }
    public LevelViewer LevelViewer { get; }
    public EnemiesViewer EnemiesViewer { get; }
    
    private GamePresenter _gamePresenter;
     Game _game { get; set; } 
    
    public event Action<Keys> KeyPressed;
    
    public Point CameraOffset;
    public Point MouseCellPos;
    public bool IsMouseOverForm;
    
    public GameForm()
    {
        //Тут нужен gamepresenter?
        PlayerViewer = new PlayerViewer();
        LevelViewer = new LevelViewer();
        EnemiesViewer = new EnemiesViewer();
        
        
        InitializeComponent();
        this.DoubleBuffered = true;
        this.Paint += new PaintEventHandler(OnPaint);
        this.KeyDown += GameForm_KeyDown;
        this.MouseMove += GameForm_MouseMove;
        this.MouseClick += GameForm_MouseClick;
        this.MouseEnter += (s, e) => IsMouseOverForm = true;
        this.MouseLeave += (s, e) => { IsMouseOverForm = false; _gamePresenter?.ClearAttack(); };
    }

    public void SetPresenter(GamePresenter presenter)
    {
        _gamePresenter = presenter;
        CameraOffset=_gamePresenter.GetCameraOffset(this.ClientSize);
    }

    public void SetGame(Game game)
    {
        _game = game;
        UpdateCameraOffset();
    }

    private void GameForm_KeyDown(object sender, KeyEventArgs e)
    {
        _gamePresenter?.OnKeyDown(e);
    }

    private void GameForm_MouseMove(object sender, MouseEventArgs e)
    {
        if (_gamePresenter != null && _game?.Player != null)
        {
            var cellSize = _constants.CellSize;
            var cellX = (e.X - CameraOffset.X) / cellSize;
            var cellY = (e.Y - CameraOffset.Y) / cellSize;
            MouseCellPos = new Point(cellX, cellY);
            _gamePresenter.UpdateAttackPreview(cellX, cellY);
        }
    }

    private void GameForm_MouseClick(object sender, MouseEventArgs e)
    {
        if (_gamePresenter != null && _game?.Player != null)
        {
            var cellSize = _constants.CellSize;
            var cellX = (e.X - CameraOffset.X) / cellSize;
            var cellY = (e.Y - CameraOffset.Y) / cellSize;
            _gamePresenter.ExecuteAttack(cellX, cellY);
        }
    }

    public void Redraw()
    {
        UpdateCameraOffset();
        this.Invalidate();
    }

    private void UpdateCameraOffset()
    {
        if (_gamePresenter != null && _game != null)
        {
            CameraOffset = _gamePresenter.GetCameraOffset(this.ClientSize);
        }
    }

    private void OnPaint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        
        if (_game?.Level != null)
            LevelViewer.DrawLevel(g, _game.Level, CameraOffset);
        
        if (_game?.Player != null)
            PlayerViewer.DrawPlayer(g, _game.Player, CameraOffset);
        
        if (_game?.Enemies != null)
            EnemiesViewer.DrawEnemies(g, _game.Enemies, CameraOffset);
        
        // Draw attack preview
        if (_game?.Player != null && _game.Player.IsAttacking && IsMouseOverForm)
            PlayerViewer.DrawAttackPreview(g, _game.Player, CameraOffset);
    }
}