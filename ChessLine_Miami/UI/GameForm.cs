using ChessLine_Miami.Models;
using ChessLine_Miami.Presenters;
using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Forms;
namespace ChessLine_Miami.UI;
class _constants{
    public const int CellSize = 100;
}

public partial class GameForm : Form, IGameView
{
    private System.Windows.Forms.Timer _animationTimer;
    public PlayerViewer PlayerViewer { get; }
    public LevelViewer LevelViewer { get; }
    public EnemiesViewer EnemiesViewer { get; }
    public MenuViewer MenuViewer { get; } 
    
    private GamePresenter _gamePresenter;
     Game _game { get; set; } 
    private bool _isRPressed;
    
    // private bool _isPaused;
    // private bool _isShowingTutorial;
    // private int _tutorialImageIndex; // 0 = WalkGuide, 1 = AttackGuide
    // private Rectangle _tutorialButtonRect;
    // private Rectangle _pauseResumeButtonRect;
    // private Rectangle _pauseRestartButtonRect;
    // private Rectangle _pauseExitButtonRect;
    
    public event Action<Keys> KeyPressed;
    
    public Point CameraOffset;
    public Point MouseCellPos;
    public bool IsMouseOverForm;
    
    public GameForm()
    {
       
        PlayerViewer = new PlayerViewer();
        LevelViewer = new LevelViewer();
        EnemiesViewer = new EnemiesViewer();
        MenuViewer = new MenuViewer();
        
        
        InitializeComponent();
        this.DoubleBuffered = true;
        this.Paint += new PaintEventHandler(OnPaint);
        this.KeyDown += GameForm_KeyDown;
        this.KeyUp += GameForm_KeyUp;
        this.MouseMove += GameForm_MouseMove;
        this.MouseClick += GameForm_MouseClick;
        this.FormClosed += (s, e) => _animationTimer?.Dispose();
        this.MouseEnter += (s, e) => IsMouseOverForm = true;
        this.MouseLeave += (s, e) => { IsMouseOverForm = false; _gamePresenter?.ClearAttack(); };
        _animationTimer = new System.Windows.Forms.Timer();
        _animationTimer.Interval = 16; // 60 кадров в секунду 
        _animationTimer.Tick += (s, e) => 
    {
        // Принудительно обновляем только интерфейс
        this.Invalidate(); 
    };
    _animationTimer.Start();
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
        if (e.KeyCode == Keys.Escape)
        {
            MenuViewer.IsPaused = !MenuViewer.IsPaused;
            e.Handled = true;
            return;
        }

        if (MenuViewer.IsPaused || MenuViewer.IsShowingTutorial)
            return;

        if (e.KeyCode == Keys.R)
        {
            _isRPressed = true;
        }

        _gamePresenter?.OnKeyDown(e, _isRPressed);
    }

    private void GameForm_KeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.R)
        {
            _isRPressed = false;
        }
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
        if (MenuViewer.IsShowingTutorial)
        {
            MenuViewer.TutorialImageIndex = (MenuViewer.TutorialImageIndex + 1) % 2;
            if (MenuViewer.TutorialImageIndex == 0)
                MenuViewer.IsShowingTutorial = false;
            return;
        }

        if (MenuViewer.IsPaused)
        {
            if (MenuViewer._pauseResumeButtonRect.Contains(e.Location))
            {
                MenuViewer.IsPaused = false;
            }
            else if (MenuViewer._pauseRestartButtonRect.Contains(e.Location))
            {
                MenuViewer.IsPaused = false;
                _gamePresenter?.RestartLevel();
            }
            else if (MenuViewer._pauseExitButtonRect.Contains(e.Location))
            {
                this.Close();
            }
            return;
        }

        // Tutorial button for first level
        if (MenuViewer._tutorialButtonRect.Contains(e.Location) && _game?.Level?.Name == "Level1")
        {
            MenuViewer.IsShowingTutorial = true;
            MenuViewer.TutorialImageIndex = 0;
            return;
        }

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
        {
            PlayerViewer.DrawPlayer(g, _game.Player, CameraOffset);
            PlayerViewer.DrawRushBar(g, _game.Player, _isRPressed);
        }
        
        if (_game?.Enemies != null)
            EnemiesViewer.DrawEnemies(g, _game.Enemies, CameraOffset);
        
        // Draw attack preview
        if (_game?.Player != null && _game.Player.IsAttacking && IsMouseOverForm && !MenuViewer.IsPaused && !MenuViewer.IsShowingTutorial)
            PlayerViewer.DrawAttackPreview(g, _game.Player, CameraOffset);

        // Draw tutorial button for first level
        if (_game?.Level?.Name == "Level1")
        {
            MenuViewer.DrawTutorialButton(g, _game, this);
        }

        // Draw tutorial screen
        if (MenuViewer.IsShowingTutorial)
        {
            MenuViewer.DrawTutorialScreen(g, this);
        }

        // Пауза
        if (MenuViewer.IsPaused)
        {
            MenuViewer.DrawPauseMenu(g, this);
        }
    }

}