using ChessLine_Miami.Models;
using ChessLine_Miami.Presenters;
using System;
using System.Windows.Forms;
namespace ChessLine_Miami.UI;
public partial class GameForm : Form, IGameView,
{
    private PlayerViewer PlayerViewer { get; }
    private LevelViewer LevelViewer { get; }
    
    public Level Level { get;}
    

    
    
    public GameForm()
    {
        PlayerViewer = new PlayerViewer();
        LevelViewer = new LevelViewer();
        this.KeyDown += (s, e) => KeyPressed?.Invoke(e.KeyCode);

        InitializeComponent();
    // Включаем двойную буферизацию, чтобы картинка не мерцала
    this.DoubleBuffered = true;
    this.Paint += new PaintEventHandler(OnPaint);
    
    }

    private void OnPaint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        
        // Пример рисования сетки или игрока
        g.FillRectangle(Brushes.Green, 50, 50, 100, 100); // Рисуем квадрат
        g.DrawEllipse(Pens.Red, 200, 10, 50, 50);        // Рисуем круг
    }
}