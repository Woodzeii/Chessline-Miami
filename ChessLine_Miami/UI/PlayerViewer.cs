using System.IO;
using ChessLine_Miami.Presenters;
using ChessLine_Miami.Models;
using System.Drawing;
namespace ChessLine_Miami.UI;


public class PlayerViewer
{
    Image playerImg = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI/Photo/pawn.png")); 
    public void DrawPlayer(Graphics g, Player player, Point cameraOffset)
    {
        var cellSize = _constants.CellSize;
        var playerRect = new RectangleF(
            player.RenderFieldPos.X * cellSize + cameraOffset.X, 
            player.RenderFieldPos.Y * cellSize + cameraOffset.Y, 
            cellSize, 
            cellSize
        );
        g.DrawImage(playerImg, playerRect);
    }

    public void DrawAttackPreview(Graphics g, Player player, Point cameraOffset)
    {
        if (!player.IsAttacking) return;

        var cellSize = _constants.CellSize;
        var target = player.AttackTarget;

        // Проверка цель-диагональ?
        var dx = Math.Abs(target.X - player.FieldPos.X);
        var dy = Math.Abs(target.Y - player.FieldPos.Y);

        // Показывать атаки 
        if (dx == dy && dx ==1 )
        {
            var rect = new Rectangle(
                target.X * cellSize + cameraOffset.X,
                target.Y * cellSize + cameraOffset.Y,
                cellSize,
                cellSize
            );

            // Красное выделение цели
            using (var brush = new SolidBrush(Color.FromArgb(100, 255, 0, 0)))
            {
                g.FillRectangle(brush, rect);
            }

            // Граница
            using (var pen = new Pen(Color.Red, 3))
            {
                g.DrawRectangle(pen, rect);
            }
        }
    }
    
    public void DrawRushBar(Graphics g, Player player, bool isRPressed)
{

    int barX = 25;
    int barY = 25;
    int barWidth = 300;
    int barHeight = 24;


    float current = Player.StepsToRush - player.RushCooldown;
    float max = Player.StepsToRush;
    float fillPercentage = max > 0 ? current / max : 0;
    fillPercentage = Math.Clamp(fillPercentage, 0, 1);
    bool isReady = fillPercentage >= 1.0f;

    // Вводим время для анимации глитча и мигания
    double time = DateTime.Now.TimeOfDay.TotalSeconds;
    
    // Эффект глитча (случайное легкое дрожание координат, если рывок готов)
    int glitchX = 0;
    int glitchY = 0;
    if (isReady)
    {
       
        var rand = new Random();
        if (rand.Next(0, 100) < 15) // 15% шанс сдвига в каждом кадре
        {
            glitchX = rand.Next(-2, 3);
            glitchY = rand.Next(-1, 2);
        }
    }

    // Применяем сдвиг глитча ко всей панели
    barX += glitchX;
    barY += glitchY;

    // 1. ЗАДНИЙ ФОН 
    Color hmMiamiDark = Color.FromArgb(25, 10, 35);
    using (SolidBrush bgBrush = new SolidBrush(hmMiamiDark))
    {
        g.FillRectangle(bgBrush, barX, barY, barWidth, barHeight);
    }

    // 2. ЗАПОЛНЕНИЕ ШКАЛЫ
    int currentFillWidth = (int)(barWidth * fillPercentage);
    if (currentFillWidth > 0)
    {
        Color barColor;

        if (isReady)
        {
            if (isRPressed)
            {
                // Если клавиша R зажата и заряжено -мигаем
                barColor = (int)(time * 15) % 2 == 0 ? Color.White : Color.FromArgb(255, 0, 128);
            }
            else
            {
                // Заряд готов: плавно переливается 
                int r = (int)(127 + 128 * Math.Sin(time * 4));
                int gVal = (int)(190 + 65 * Math.Cos(time * 4));
                int b = (int)(220 + 35 * Math.Sin(time * 4));
                barColor = Color.FromArgb(Math.Clamp(r, 0, 255), Math.Clamp(gVal, 0, 255), Math.Clamp(b, 0, 255));
            }
        }
        else
        {
            
            barColor = Color.FromArgb(255, 60, 0);
        }

        using (SolidBrush fillBrush = new SolidBrush(barColor))
        {
            g.FillRectangle(fillBrush, barX, barY, currentFillWidth, barHeight);
        }
    }

    //Рамка
    Color borderColor = isReady ? Color.FromArgb(255, 0, 128) : Color.FromArgb(90, 30, 120);
    using (Pen borderPen = new Pen(borderColor, 3)) 
    {
        g.DrawRectangle(borderPen, barX, barY, barWidth, barHeight);
    }

    // 4. Текст Rush и проценты
    int textX = barX + barWidth + 12 + (isRPressed ? new Random().Next(-1, 2) : 0);
    int textY = barY;

    using (Font font = new Font("Impact", 15, FontStyle.Italic)) 
    {
        string text = isRPressed && isReady ? "READY TO RUSH!" : $"RUSH: {(int)(fillPercentage * 100)}%";
        Color textColor = isReady ? Color.FromArgb(0, 255, 255) : Color.FromArgb(200, 200, 200);

        // Рисуем черную тень для текста 
        using (SolidBrush shadowBrush = new SolidBrush(Color.Black))
        {
            g.DrawString(text, font, shadowBrush, textX + 2, textY + 2);
        }

        // Рисуем основной текст
        using (SolidBrush textBrush = new SolidBrush(textColor))
        {
            g.DrawString(text, font, textBrush, textX, textY);
        }
    }
}
}