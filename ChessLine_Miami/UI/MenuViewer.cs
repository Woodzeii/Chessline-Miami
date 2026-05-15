using ChessLine_Miami.Models;
using ChessLine_Miami.Presenters;
using System.IO;


public class MenuViewer
{
    public bool IsPaused { get;  set; }
    public bool IsShowingTutorial { get;  set; }
    public int TutorialImageIndex { get;  set; } // 0 = WalkGuide, 1 = AttackGuide
    public Rectangle _tutorialButtonRect;
    public Rectangle _pauseResumeButtonRect;
    public Rectangle _pauseRestartButtonRect;
    public Rectangle _pauseExitButtonRect;
    // Кнопка гайда
        public void DrawTutorialButton(Graphics g, Game game,Form form)
        {
            _tutorialButtonRect = new Rectangle(10, form.ClientSize.Height - 60, 100, 50);
            using var buttonBrush = new SolidBrush(Color.FromArgb(100, 100, 150));
            using var buttonPen = new Pen(Color.White, 2);
            g.FillRectangle(buttonBrush, _tutorialButtonRect);
            g.DrawRectangle(buttonPen, _tutorialButtonRect);
            using var font = new Font("Arial", 12, FontStyle.Bold);
            g.DrawString("Tutorial", font, Brushes.White, _tutorialButtonRect.X + 15, _tutorialButtonRect.Y + 15);
        }

        // Отображение туториала
        public void DrawTutorialScreen(Graphics g, Form form)
        {
            using var dimBrush = new SolidBrush(Color.FromArgb(200, 0, 0, 0));
            g.FillRectangle(dimBrush, form.ClientRectangle);

            string imagePath = TutorialImageIndex == 0
                ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI/Photo/guide/WalkGuide.png")
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI/Photo/guide/AttackGuide.png");

            if (File.Exists(imagePath))
            {
                using var tutorialImage = Image.FromFile(imagePath);
                var imageWidth = 1536/2;
                var imageHeight = 1024/2;
                var x = (form.ClientSize.Width - imageWidth) / 2;
                var y = (form.ClientSize.Height - imageHeight) / 2;
                g.DrawImage(tutorialImage, x, y, imageWidth, imageHeight);
            }

            using var font = new Font("Arial", 14, FontStyle.Bold);
            var instructionText = TutorialImageIndex == 0 ? "Click to see Attack Guide" : "Click to continue";
            var textSize = g.MeasureString(instructionText, font);
            g.DrawString(instructionText, font, Brushes.White, 
                (form.ClientSize.Width - textSize.Width) / 2, 
                form.ClientSize.Height - 60);
        }

        // Пауза
        public void DrawPauseMenu(Graphics g, Form form)
        {
            using var dimBrush = new SolidBrush(Color.FromArgb(200, 0, 0, 0));
            g.FillRectangle(dimBrush, form.ClientRectangle);

            var menuWidth = 300;
            var menuHeight = 250;
            var menuX = (form.ClientSize.Width - menuWidth) / 2;
            var menuY = (form.ClientSize.Height - menuHeight) / 2;

            using var menuBrush = new SolidBrush(Color.FromArgb(50, 50, 80));
            using var menuPen = new Pen(Color.White, 2);
            g.FillRectangle(menuBrush, menuX, menuY, menuWidth, menuHeight);
            g.DrawRectangle(menuPen, menuX, menuY, menuWidth, menuHeight);

            using var titleFont = new Font("Arial", 16, FontStyle.Bold);
            g.DrawString("PAUSED", titleFont, Brushes.White, menuX + 100, menuY + 20);

            var buttonWidth = 200;
            var buttonHeight = 40;
            var buttonX = menuX + (menuWidth - buttonWidth) / 2;

            // Продолжить
            _pauseResumeButtonRect = new Rectangle(buttonX, menuY + 70, buttonWidth, buttonHeight);
            DrawButton(g, _pauseResumeButtonRect, "Resume");

            // Начать заново
            _pauseRestartButtonRect = new Rectangle(buttonX, menuY + 120, buttonWidth, buttonHeight);
            DrawButton(g, _pauseRestartButtonRect, "Restart Level");

            // Выйти из игры
            _pauseExitButtonRect = new Rectangle(buttonX, menuY + 170, buttonWidth, buttonHeight);
            DrawButton(g, _pauseExitButtonRect, "Exit Game");
        }
    

    private void DrawButton(Graphics g, Rectangle rect, string text)
    {
        using var buttonBrush = new SolidBrush(Color.FromArgb(80, 80, 120));
        using var buttonPen = new Pen(Color.FromArgb(150, 150, 255), 2);
        g.FillRectangle(buttonBrush, rect);
        g.DrawRectangle(buttonPen, rect);
        
        using var font = new Font("Arial", 12, FontStyle.Bold);
        var textSize = g.MeasureString(text, font);
        g.DrawString(text, font, Brushes.White,
            rect.X + (rect.Width - textSize.Width) / 2,
            rect.Y + (rect.Height - textSize.Height) / 2);
    }
}