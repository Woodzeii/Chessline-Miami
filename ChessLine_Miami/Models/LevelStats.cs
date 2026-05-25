namespace ChessLine_Miami.Models;

public class LevelStats
{
    public int TimeSeconds { get; set; }
    public int ComboKills { get; set; }
    public int TotalKills { get; set; }
    public double Rating { get; set; } // Оценка от 1 до 5 звёзд
    public string RatingText { get; set; } // ★★★☆☆
    
    public LevelStats()
    {
        TimeSeconds = 0;
        ComboKills = 0;
        TotalKills = 0;
        Rating = 0;
        RatingText = "";
    }

    public void CalculateRating()
    {
        // Базовая оценка из времени
        double timeRating = 5.0 - (TimeSeconds / 60.0); // За каждую минуту -1 звёзда
        timeRating = Math.Max(1, Math.Min(5, timeRating)); // от 1 до 5

        // Бонус за комбо
        double comboBonus = (ComboKills / (double)Math.Max(1, TotalKills)) * 0.5; // До +0.5 звёзд за комбо

        Rating = timeRating + comboBonus;
        Rating = Math.Min(5, Rating); // Максимум 5 звёзд

        // Создаём текстовое представление
        int fullStars = (int)Math.Floor(Rating);
        bool hasHalfStar = (Rating - fullStars) >= 0.5;
        int emptyStars = 5 - fullStars - (hasHalfStar ? 1 : 0);

        RatingText = new string('★', fullStars) + (hasHalfStar ? "½" : "") + new string('☆', emptyStars);
    }
}
