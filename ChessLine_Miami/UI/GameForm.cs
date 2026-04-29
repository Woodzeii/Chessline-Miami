using ChessLine_Miami.Models;
using ChessLine_Miami.Presenters;
using System;
using System.Windows.Forms;
namespace ChessLine_Miami.UI;
public partial class GameForm : Form, IGameView
{
    public PlayerViewer PlayerViewer { get; }
    public LevelViewer LevelViewer { get; }
    public EnemiesViewer EnemiesViewer { get; }
    
    private GamePresenter _gamePresenter;
     Game _game { get; set; } 
    
    public event Action<Keys> KeyPressed;
    
   
    public Point CameraOffset;
    
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
    }

    public void SetPresenter(GamePresenter presenter)
    {
        _gamePresenter = presenter;
        CameraOffset=_gamePresenter.GetCameraOffset(this.ClientSize);
    }

    public void SetGame(Game game)
    {
        _game = game;
    }

    private void GameForm_KeyDown(object sender, KeyEventArgs e)
    {
        _gamePresenter?.OnKeyDown(e);
    }

    public void Redraw()
    {
        this.Invalidate();
    }

    private void OnPaint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        
        if (_game?.Level != null)
            LevelViewer.DrawLevel(g, _game.Level, CameraOffset);
        
        if (_game?.Player != null)
            PlayerViewer.DrawPlayer(g, _game.Player);
    }
}