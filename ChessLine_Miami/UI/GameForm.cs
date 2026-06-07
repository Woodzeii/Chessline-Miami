using ChessLine_Miami.Models;
using ChessLine_Miami.Presenters;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Controls;
using System.Windows.Forms;
namespace ChessLine_Miami.UI;
class _constants{
    public const int CellSize = 100;
}

public partial class GameForm : Form, IGameView
{
    public const float FrameUpdateSpeed = 0.20f;
    private System.Windows.Forms.Timer _animationTimer;
    public PlayerViewer PlayerViewer { get; }
    public LevelViewer LevelViewer { get; }
    public EnemiesViewer EnemiesViewer { get; }
    public MenuViewer MenuViewer { get; } 
    
    private GamePresenter _gamePresenter;
     Game _game { get; set; }
    private bool _isRPressed;
    private DateTime _levelTitleVisibleUntil = DateTime.MinValue;
    private const double LevelTitleDisplayDurationSeconds = 2.5;
    
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
        _animationTimer.Interval = 20; // 50 FPS
        _animationTimer.Tick += (s, e) => 
    {
        _gamePresenter?.UpdateAnimations();
        UpdateCameraOffset();
        this.Invalidate(); 
    };
    _animationTimer.Start();
    }

    private PointF _currentCameraOffsetF;

    public void SetPresenter(GamePresenter presenter)
    {
        _gamePresenter = presenter;
        _currentCameraOffsetF = _gamePresenter.GetCameraOffsetF(this.ClientSize);
        CameraOffset = new Point((int)Math.Round(_currentCameraOffsetF.X), (int)Math.Round(_currentCameraOffsetF.Y));
    }

    public void SetGame(Game game)
    {
        _game = game;
        _levelTitleVisibleUntil = DateTime.Now.AddSeconds(LevelTitleDisplayDurationSeconds);
        UpdateCameraOffset();
    }

    private void GameForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            if (MenuViewer.IsShowingMainMenu || MenuViewer.IsShowingLevelComplete || MenuViewer.IsShowingLevelSelection)
                return; // Не обрабатываем Escape в меню
            
            MenuViewer.IsPaused = !MenuViewer.IsPaused;
            e.Handled = true;
            return;
        }

        if (MenuViewer.IsShowingMainMenu || MenuViewer.IsPaused || MenuViewer.IsShowingTutorial || MenuViewer.IsShowingLevelComplete || MenuViewer.IsShowingLevelSelection)
            return;

        if (_levelTitleVisibleUntil > DateTime.Now)
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
        if (MenuViewer.IsShowingMainMenu || MenuViewer.IsPaused || MenuViewer.IsShowingTutorial || MenuViewer.IsShowingLevelComplete || MenuViewer.IsShowingLevelSelection)
            return;

        if (_levelTitleVisibleUntil > DateTime.Now)
            return;

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
        // Экран настроек
        if (MenuViewer.IsShowingSettings)
        {
            // Кнопка уменьшения громкости
            if (MenuViewer._volumeDecreaseButtonRect.Contains(e.Location))
            {
                Program.PlayerProgress.Volume = Math.Max(0, Program.PlayerProgress.Volume - 0.1);
                Program.PlayerProgress.SaveProgress();
                SFX.UpdateVolume();
                this.Invalidate();
                return;
            }

            // Кнопка увеличения громкости
            if (MenuViewer._volumeIncreaseButtonRect.Contains(e.Location))
            {
                Program.PlayerProgress.Volume = Math.Min(1.0, Program.PlayerProgress.Volume + 0.1);
                Program.PlayerProgress.SaveProgress();
                SFX.UpdateVolume();
                this.Invalidate();
                return;
            }

            // Чекбокс простой отрисовки лавы
            if (MenuViewer._lavaCheckboxRect.Contains(e.Location))
            {
                Program.PlayerProgress.UseSimpleLavaTiles = !Program.PlayerProgress.UseSimpleLavaTiles;
                Program.PlayerProgress.SaveProgress();
                this.Invalidate();
                return;
            }

            // Клик на слайдер громкости
            if (MenuViewer._volumeSliderRect.Contains(e.Location))
            {
                var sliderRelativeX = e.X - MenuViewer._volumeSliderRect.X;
                Program.PlayerProgress.Volume = Math.Max(0, Math.Min(1.0, (double)sliderRelativeX / MenuViewer._volumeSliderRect.Width));
                Program.PlayerProgress.SaveProgress();
                SFX.UpdateVolume();
                this.Invalidate();
                return;
            }

            // Кнопка "Назад"
            if (MenuViewer._settingsBackButtonRect.Contains(e.Location))
            {
                MenuViewer.IsShowingSettings = false;
                MenuViewer.IsShowingMainMenu = true;
                this.Invalidate();
                return;
            }
            return;
        }

        // Экран выбора уровней
        if (MenuViewer.IsShowingLevelSelection)
        {
            // Проверяем клик на кнопку "Назад"
            if (MenuViewer._levelSelectionBackButtonRect.Contains(e.Location))
            {
                MenuViewer.IsShowingLevelSelection = false;
                MenuViewer.IsShowingMainMenu = true;
                this.Invalidate();
                return;
            }

            // Проверяем клик на кнопки уровней
            for (int i = 0; i < MenuViewer._levelButtonRects.Count; i++)
            {
                if (MenuViewer._levelButtonRects[i].Contains(e .Location))
                {
                    MenuViewer.IsShowingLevelSelection = false;
                    Program.LoadLevel(i);
                    this.Invalidate();
                    return;
                }
            }
            return;
        }

        // Экран завершения уровня
        if (MenuViewer.IsShowingLevelComplete)
        {
            if (MenuViewer._levelCompleteNextButtonRect.Contains(e.Location))
            {
                MenuViewer.IsShowingLevelComplete = false;
                // Переходим на следующий уровень
                _gamePresenter?.OnLevelComplete();
            }
            else if (MenuViewer._levelCompleteMenuButtonRect.Contains(e.Location))
            {
                _gamePresenter?.OnLevelComplete();
                MenuViewer.IsShowingLevelComplete = false;
                ShowMainMenu();
            }
            return;
        }

        // Главное меню
        if (MenuViewer.IsShowingMainMenu)
        {
            if (MenuViewer._mainMenuStartButtonRect.Contains(e.Location))
            {
                MenuViewer.IsShowingMainMenu = false;
                Program.LoadNextLevel();
                this.Invalidate();
            }
            else if (MenuViewer._mainMenuRoomsButtonRect.Contains(e.Location))
            {
                MenuViewer.IsShowingMainMenu = false;
                MenuViewer.IsShowingLevelSelection = true;
                this.Invalidate();
            }
            else if (MenuViewer._mainMenuSettingsButtonRect.Contains(e.Location))
            {
                MenuViewer.IsShowingMainMenu = false;
                MenuViewer.IsShowingSettings = true;
                this.Invalidate();
            }
            else if (MenuViewer._mainMenuExitButtonRect.Contains(e.Location))
            {
                this.Close();
            }
            return;
        }

        if (_levelTitleVisibleUntil > DateTime.Now)
        {
            return;
        }

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
                ShowMainMenu();
            }
            return;
        }

        // Tutorial button for first level
        if (MenuViewer._tutorialButtonRect.Contains(e.Location) && _game?.Level?.Name == "StartLevel")
        {
            MenuViewer.IsShowingTutorial = true;
            MenuViewer.TutorialImageIndex = 0;
            return;
        }

        if (_levelTitleVisibleUntil > DateTime.Now)
        {
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
            var target = _gamePresenter.GetCameraOffsetF(this.ClientSize);
            _currentCameraOffsetF = new PointF(
                _currentCameraOffsetF.X + (target.X - _currentCameraOffsetF.X) * FrameUpdateSpeed,
                _currentCameraOffsetF.Y + (target.Y - _currentCameraOffsetF.Y) * FrameUpdateSpeed
            );

            if (Math.Abs(target.X - _currentCameraOffsetF.X) < 0.5f)
                _currentCameraOffsetF.X = target.X;
            if (Math.Abs(target.Y - _currentCameraOffsetF.Y) < 0.5f)
                _currentCameraOffsetF.Y = target.Y;

            CameraOffset = new Point((int)Math.Round(_currentCameraOffsetF.X), (int)Math.Round(_currentCameraOffsetF.Y));
        }
    }

    private void OnPaint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        
        // Главное меню
        if (MenuViewer.IsShowingMainMenu)
        {
            MenuViewer.DrawMainMenu(g, this);
            return;
        }

        // Экран настроек
        if (MenuViewer.IsShowingSettings)
        {
            MenuViewer.DrawSettingsScreen(g, this, Program.PlayerProgress);
            return;
        }

        // Экран выбора уровней
        if (MenuViewer.IsShowingLevelSelection)
        {
            MenuViewer.DrawLevelSelectionScreen(g, this, Program.PlayerProgress);
            return;
        }

        // Экран завершения уровня
        if (MenuViewer.IsShowingLevelComplete)
        {
            MenuViewer.DrawLevelCompleteScreen(g, this, _game?.Stats);
            return;
        }
        
        if (_game?.Level != null)
            LevelViewer.DrawLevel(g, _game.Level, CameraOffset);
        
        
        
        if (_game?.Enemies != null)
            EnemiesViewer.DrawEnemies(g, _game.Enemies, CameraOffset);
            
        if (_game?.Player != null)
        {
            PlayerViewer.DrawPlayer(g, _game.Player, CameraOffset);
            PlayerViewer.DrawRushBar(g, _game.Player, _isRPressed);
        }
        // Draw attack preview
        if (_game?.Player != null && _game.Player.IsAttacking && IsMouseOverForm && !MenuViewer.IsPaused && !MenuViewer.IsShowingTutorial)
            PlayerViewer.DrawAttackPreview(g, _game.Player, CameraOffset);

        // Draw tutorial button for first level
        if (_game?.Level?.Name == "StartLevel")
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

        // Название левелочка
        if (_levelTitleVisibleUntil > DateTime.Now && _game?.Level != null)
        {
            DrawLevelTitle(g);
        }
    }

    private void DrawLevelTitle(Graphics g)
    {
        var title = _game.Level.Name;
        using var titleFont = new Font("Segoe UI", 32, FontStyle.Bold);
        var textSize = g.MeasureString(title, titleFont);
        var x = (ClientSize.Width - textSize.Width) / 2f;
        var y = (ClientSize.Height - textSize.Height) / 2f;

        using var backgroundBrush = new SolidBrush(Color.FromArgb(180, 0, 0, 0));
        var padding = 20;
        g.FillRectangle(backgroundBrush, x - padding, y - padding, textSize.Width + padding * 2, textSize.Height + padding * 2);
        g.DrawString(title, titleFont, Brushes.White, x, y);
    }

    public void OnLevelComplete()
    {
        // Показываем экран результатов
        MenuViewer.IsShowingLevelComplete = true;
        this.Invalidate();
    }

    public void ShowMainMenu()
    {
        MenuViewer.IsShowingMainMenu = true;
        MenuViewer.IsShowingLevelSelection = false;
        MenuViewer.IsShowingLevelComplete = false;
        MenuViewer.IsPaused = false;
        SFX.PlayMenuMusic();
        this.Invalidate();
    }
}