using ChessLine_Miami.Models;
using ChessLine_Miami.Presenters;
using System.IO;
using NAudio.Wave;


public class MenuViewer
{
    // Main Menu
    public bool IsShowingMainMenu { get; set; } = true;
    public bool IsShowingLevelSelection { get; set; }
    public bool IsShowingSettings { get; set; }
    public Rectangle _mainMenuStartButtonRect;
    public Rectangle _mainMenuRoomsButtonRect;
    public Rectangle _mainMenuSettingsButtonRect;
    public Rectangle _mainMenuExitButtonRect;

    // Settings
    public Rectangle _settingsBackButtonRect;
    public Rectangle _volumeSliderRect;
    public Rectangle _volumeDecreaseButtonRect;
    public Rectangle _volumeIncreaseButtonRect;
    public Rectangle _lavaCheckboxRect;

    // Level Selection
    public List<Rectangle> _levelButtonRects = new();
    public Rectangle _levelSelectionBackButtonRect;

    // Game states
    public bool IsPaused { get;  set; }
    public bool IsShowingTutorial { get;  set; }
    public bool IsShowingLevelComplete { get; set; }
    public int TutorialImageIndex { get;  set; } // 0 = WalkGuide, 1 = AttackGuide, 2 = Strategy
    public Rectangle _tutorialButtonRect;
    public Rectangle _pauseResumeButtonRect;
    public Rectangle _pauseRestartButtonRect;
    public Rectangle _pauseExitButtonRect;
    public Rectangle _levelCompleteNextButtonRect;
    public Rectangle _levelCompleteMenuButtonRect;
    
    // Главное меню
    public void DrawMainMenu(Graphics g, Form form)
    {
        // Фон
        // using var backgroundBrush = new SolidBrush(Color.FromArgb(30, 30, 50));
        // g.FillRectangle(backgroundBrush, form.ClientRectangle);

        string backgroundImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI/Photo/bgpurple.gif");
        var _backgroundImage = Image.FromFile(backgroundImagePath);
        g.DrawImage(_backgroundImage, form.ClientRectangle);
            
        var menuWidth = 400;
        var menuHeight = 400;
        var menuX = (form.ClientSize.Width - menuWidth) / 2;
        var menuY = (form.ClientSize.Height - menuHeight) / 2+200;

        
        
        // Заголовок
        using var titleFont = new Font("Arial", 24, FontStyle.Bold);
        //g.DrawString("CHESSLINE MIAMI", titleFont, Brushes.White, menuX + 50, menuY + 20);
        g.DrawImage(Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI/Photo/LogoCM.png")), menuX-240 , menuY-470, 900, 900*2/3);

        var buttonWidth = 250;
        var buttonHeight = 50;
        var buttonX = menuX + (menuWidth - buttonWidth) / 2;

        // Кнопка "Начать игру"
        _mainMenuStartButtonRect = new Rectangle(buttonX, menuY + 80, buttonWidth, buttonHeight);
        DrawMainMenuButton(g, _mainMenuStartButtonRect, "Start Game");

        // Кнопка "Комнаты"
        _mainMenuRoomsButtonRect = new Rectangle(buttonX, menuY + 150, buttonWidth, buttonHeight);
        DrawMainMenuButton(g, _mainMenuRoomsButtonRect, "Rooms");

        // Кнопка "Настройки"
        _mainMenuSettingsButtonRect = new Rectangle(buttonX, menuY + 220, buttonWidth, buttonHeight);
        DrawMainMenuButton(g, _mainMenuSettingsButtonRect, "Settings");

        // Кнопка "Выйти"
        _mainMenuExitButtonRect = new Rectangle(buttonX, menuY + 290, buttonWidth, buttonHeight);
        DrawMainMenuButton(g, _mainMenuExitButtonRect, "Exit Game");
    }

    private void DrawMainMenuButton(Graphics g, Rectangle rect, string text)
    {
        using var buttonBrush = new SolidBrush(Color.FromArgb(70, 70, 110));
        using var buttonPen = new Pen(Color.FromArgb(150, 150, 255), 2);
        g.FillRectangle(buttonBrush, rect);
        g.DrawRectangle(buttonPen, rect);
        
        using var font = new Font("Arial", 13, FontStyle.Bold);
        var textSize = g.MeasureString(text, font);
        g.DrawString(text, font, Brushes.White,
            rect.X + (rect.Width - textSize.Width) / 2,
            rect.Y + (rect.Height - textSize.Height) / 2);
    }
    
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

            string imagePath;
            switch (TutorialImageIndex)
            {
                case 1:
                    imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI/Photo/guide/AttackGuide.png");
                    break;
                case 2:
                    imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI/Photo/guide/Strategy.png");
                    break;
                default:
                    imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI/Photo/guide/WalkGuide.png");
                    break;
            }

            if (File.Exists(imagePath))
            {
                using var tutorialImage = Image.FromFile(imagePath);
                var imageWidth = 1536 / 2;
                var imageHeight = 1024 / 2;
                var x = (form.ClientSize.Width - imageWidth) / 2;
                var y = (form.ClientSize.Height - imageHeight) / 2;
                g.DrawImage(tutorialImage, x, y, imageWidth, imageHeight);
            }

            using var font = new Font("Arial", 14, FontStyle.Bold);
            string instructionText = TutorialImageIndex switch
            {
                0 => "Click to see Attack Guide",
                1 => "Click to see Strategy",
                2 => "Click to close",
                _ => "Click to continue"
            };

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
            var titleText = "PAUSED";
            var titleSize = g.MeasureString(titleText, titleFont);
            g.DrawString(titleText, titleFont, Brushes.White,
                menuX + (menuWidth - titleSize.Width) / 2,
                menuY + 20);

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
    
    // Экран завершения уровня
    public void DrawLevelCompleteScreen(Graphics g, Form form, LevelStats stats)
    {
        // Фон
        using var dimBrush = new SolidBrush(Color.FromArgb(200, 0, 0, 0));
        g.FillRectangle(dimBrush, form.ClientRectangle);

        var menuWidth = 500;
        var menuHeight = 450;
        var menuX = (form.ClientSize.Width - menuWidth) / 2;
        var menuY = (form.ClientSize.Height - menuHeight) / 2;

        using var menuBrush = new SolidBrush(Color.FromArgb(50, 50, 80));
        using var menuPen = new Pen(Color.White, 3);
        g.FillRectangle(menuBrush, menuX, menuY, menuWidth, menuHeight);
        g.DrawRectangle(menuPen, menuX, menuY, menuWidth, menuHeight);

        using var titleFont = new Font("Arial", 28, FontStyle.Bold);
        var titleText = "LEVEL COMPLETE!";
        var titleSize = g.MeasureString(titleText, titleFont);
        g.DrawString(titleText, titleFont, Brushes.Gold,
            menuX + (menuWidth - titleSize.Width) / 2,
            menuY + 20);

        var startY = menuY + 100;
        using var statsFont = new Font("Arial", 14, FontStyle.Bold);
        using var statsValueFont = new Font("Arial", 14, FontStyle.Regular);

        // Время
        g.DrawString("Time:", statsFont, Brushes.White, menuX + 50, startY);
        g.DrawString($"{stats.TimeSeconds}s", statsValueFont, Brushes.LimeGreen, menuX + 250, startY);
        startY += 40;

        // Килы в комбо
        g.DrawString("Best Combo:", statsFont, Brushes.White, menuX + 50, startY);
        g.DrawString($"{stats.ComboKills} kills", statsValueFont, Brushes.LimeGreen, menuX + 250, startY);
        startY += 40;

        // Всего килов
        g.DrawString("Total Kills:", statsFont, Brushes.White, menuX + 50, startY);
        g.DrawString($"{stats.TotalKills}", statsValueFont, Brushes.LimeGreen, menuX + 250, startY);
        startY += 60;

        // Оценка
        using var ratingFont = new Font("Arial", 32, FontStyle.Bold);
        var ratingSize = g.MeasureString(stats.RatingText, ratingFont);
        g.DrawString(stats.RatingText, ratingFont, Brushes.Gold,
            menuX + (menuWidth - ratingSize.Width) / 2, startY);

        // Кнопки
        var buttonWidth = 150;
        var buttonHeight = 40;
        var buttonY = menuY + menuHeight - 80;

        // Кнопка "Далее"
        _levelCompleteNextButtonRect = new Rectangle(menuX + 50, buttonY, buttonWidth, buttonHeight);
        DrawButton(g, _levelCompleteNextButtonRect, "Next");

        // Кнопка "Меню"
        _levelCompleteMenuButtonRect = new Rectangle(menuX + menuWidth - buttonWidth - 50, buttonY, buttonWidth, buttonHeight);
        DrawButton(g, _levelCompleteMenuButtonRect, "Menu");
    }

    // Экран выбора уровней
    public void DrawLevelSelectionScreen(Graphics g, Form form, PlayerProgress playerProgress)
    {
        // Фон
        using var backgroundBrush = new SolidBrush(Color.FromArgb(30, 30, 50));
        g.FillRectangle(backgroundBrush, form.ClientRectangle);

        var screenWidth = form.ClientSize.Width;
        var screenHeight = form.ClientSize.Height;

        // Заголовок
        using var titleFont = new Font("Arial", 28, FontStyle.Bold);
        var titleText = "SELECT LEVEL";
        var titleSize = g.MeasureString(titleText, titleFont);
        g.DrawString(titleText, titleFont, Brushes.Gold,
            (screenWidth - titleSize.Width) / 2,
            30);

        // Очищаем старые ректы
        _levelButtonRects.Clear();

        var buttonWidth = 250;
        var buttonHeight = 70;
        var spacing = 20;
        var gridWidth = buttonWidth * 2 + spacing;
        var startX = (screenWidth - gridWidth) / 2;
        var startY = 100;

        // Рисуем кнопки уровней
        for (int i = 0; i < playerProgress.Levels.Count; i++)
        {
            var levelData = playerProgress.Levels[i];
            int row = i / 2;
            int col = i % 2;

            var x = startX + col * (buttonWidth + spacing + 50);
            var y = startY + row * (buttonHeight + spacing);

            var rect = new Rectangle(x, y, buttonWidth, buttonHeight);
            _levelButtonRects.Add(rect);

            // Определяем цвет кнопки
            bool canPlay = i == 0 || playerProgress.Levels[i - 1].IsCompleted || i == playerProgress.GetNextUncompletedLevelIndex();
            bool isCompleted = levelData.IsCompleted;

            Color buttonColor = canPlay
                ? (isCompleted ? Color.FromArgb(70, 120, 70) : Color.FromArgb(80, 80, 120))
                : Color.FromArgb(60, 60, 60);

            using var buttonBrush = new SolidBrush(buttonColor);
            using var buttonPen = new Pen(isCompleted ? Color.Gold : Color.FromArgb(150, 150, 255), 2);
            g.FillRectangle(buttonBrush, rect);
            g.DrawRectangle(buttonPen, rect);

            // Текст уровня (имя из PlayerProgress, может быть пустым)
            using var levelFont = new Font("Arial", 14, FontStyle.Bold);
            string levelText = levelData.LevelName;
            var textSize = g.MeasureString(levelText, levelFont);
            g.DrawString(levelText, levelFont, Brushes.White,
                rect.X + (rect.Width - textSize.Width) / 2,
                rect.Y + 5);

            // Рейтинг и время
            if (isCompleted)
            {
                using var statsFont = new Font("Arial", 10, FontStyle.Regular);
                string statsText = $"★ {levelData.BestRating:F1} | {levelData.BestTime}s";
                var statsSize = g.MeasureString(statsText, statsFont);
                g.DrawString(statsText, statsFont, Brushes.Gold,
                    rect.X + (rect.Width - statsSize.Width) / 2,
                    rect.Y + 35);
            }
            else if (!canPlay)
            {
                using var lockFont = new Font("Arial", 12, FontStyle.Bold);
                g.DrawString("LOCKED", lockFont, Brushes.Red,
                    rect.X + (rect.Width - 50) / 2,
                    rect.Y + 25);
            }
        }

        // Кнопка "Назад"
        _levelSelectionBackButtonRect = new Rectangle((screenWidth - 100) / 2, screenHeight - 60, 100, 40);
        DrawButton(g, _levelSelectionBackButtonRect, "Back");
    }

    // Экран настроек
    public void DrawSettingsScreen(Graphics g, Form form, PlayerProgress playerProgress)
    {
        // Фон
        using var backgroundBrush = new SolidBrush(Color.FromArgb(30, 30, 50));
        g.FillRectangle(backgroundBrush, form.ClientRectangle);

        var menuWidth = 500;
        var menuHeight = 350;
        var menuX = (form.ClientSize.Width - menuWidth) / 2;
        var menuY = (form.ClientSize.Height - menuHeight) / 2;

        // Рамка меню
        using var menuBrush = new SolidBrush(Color.FromArgb(50, 50, 80));
        using var menuPen = new Pen(Color.FromArgb(150, 150, 255), 3);
        g.FillRectangle(menuBrush, menuX, menuY, menuWidth, menuHeight);
        g.DrawRectangle(menuPen, menuX, menuY, menuWidth, menuHeight);

        // Заголовок
        using var titleFont = new Font("Arial", 24, FontStyle.Bold);
        var titleText = "SETTINGS";
        var titleSize = g.MeasureString(titleText, titleFont);
        g.DrawString(titleText, titleFont, Brushes.White,
            menuX + (menuWidth - titleSize.Width) / 2,
            menuY + 20);

        // Громкость
        using var labelFont = new Font("Arial", 14, FontStyle.Bold);
        g.DrawString("Volume:", labelFont, Brushes.White, menuX + 40, menuY + 100);

        // Кнопка уменьшения громкости
        _volumeDecreaseButtonRect = new Rectangle(menuX + 40, menuY + 140, 50, 40);
        DrawButton(g, _volumeDecreaseButtonRect, "-");

        // Слайдер громкости
        var sliderX = menuX + 100;
        var sliderWidth = 250;
        _volumeSliderRect = new Rectangle(sliderX, menuY + 150, sliderWidth, 20);
        
        using var sliderBrush = new SolidBrush(Color.FromArgb(60, 60, 100));
        using var sliderPen = new Pen(Color.White, 1);
        g.FillRectangle(sliderBrush, _volumeSliderRect);
        g.DrawRectangle(sliderPen, _volumeSliderRect);

        // Ползунок
        var sliderPos = (int)(_volumeSliderRect.X + _volumeSliderRect.Width * playerProgress.Volume);
        using var thumbBrush = new SolidBrush(Color.LimeGreen);
        g.FillRectangle(thumbBrush, sliderPos - 5, _volumeSliderRect.Y - 2, 10, _volumeSliderRect.Height + 4);

        // Кнопка увеличения громкости
        _volumeIncreaseButtonRect = new Rectangle(menuX + 360, menuY + 140, 50, 40);
        DrawButton(g, _volumeIncreaseButtonRect, "+");

        // Отображение значения громкости
        using var valueFont = new Font("Arial", 12, FontStyle.Regular);
        var volumePercent = (int)(playerProgress.Volume * 100);
        g.DrawString($"{volumePercent}%", valueFont, Brushes.Yellow, menuX + 420, menuY + 148);

        // Простая отрисовка лавы (оптимизация) - чекбокс
        _lavaCheckboxRect = new Rectangle(menuX + 40, menuY + 200, 20, 20);
        using var checkboxPen = new Pen(Color.White, 2);
        g.DrawRectangle(checkboxPen, _lavaCheckboxRect);
        if (playerProgress.UseSimpleLavaTiles)
        {
            using var fill = new SolidBrush(Color.Orange);
            g.FillRectangle(fill, Rectangle.Inflate(_lavaCheckboxRect, -3, -3));
        }
        g.DrawString("Simple lava tiles(Optimization for FPS)", labelFont, Brushes.White, menuX + 70, menuY + 198);

        // Кнопка "Назад"
        _settingsBackButtonRect = new Rectangle(menuX + (menuWidth - 150) / 2, menuY + 270, 150, 50);
        DrawButton(g, _settingsBackButtonRect, "Back");
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