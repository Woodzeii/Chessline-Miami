using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChessLine_Miami.Models;

public class LevelProgress
{
    [JsonPropertyName("levelIndex")]
    public int LevelIndex { get; set; }
    
    [JsonPropertyName("levelName")]
    public string LevelName { get; set; }
    
    [JsonPropertyName("isCompleted")]
    public bool IsCompleted { get; set; }
    
    [JsonPropertyName("bestRating")]
    public double BestRating { get; set; }
    
    [JsonPropertyName("bestTime")]
    public int BestTime { get; set; }
    
    [JsonPropertyName("bestCombo")]
    public int BestCombo { get; set; }
    
    [JsonPropertyName("totalKills")]
    public int TotalKills { get; set; }

    public LevelProgress(int levelIndex, string levelName)
    {
        LevelIndex = levelIndex;
        LevelName = levelName;
        IsCompleted = false;
        BestRating = 0;
        BestTime = 0;
        BestCombo = 0;
        TotalKills = 0;
    }

    public void UpdateProgress(LevelStats stats)
    {
        IsCompleted = true;
        
        if (stats.Rating > BestRating)
        {
            BestRating = stats.Rating;
            BestTime = stats.TimeSeconds;
            BestCombo = stats.ComboKills;
            TotalKills = stats.TotalKills;
        }
    }
}

public class PlayerProgress
{
    [JsonPropertyName("levels")]
    public List<LevelProgress> Levels { get; set; } = new();
    
    [JsonPropertyName("currentLevelIndex")]
    public int CurrentLevelIndex { get; set; } = 0;

    public static string GetProgressFilePath()
    {
        var appDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ChessLineMiami");
        
        if (!Directory.Exists(appDataPath))
            Directory.CreateDirectory(appDataPath);
        
        return Path.Combine(appDataPath, "progress.json");
    }

    public void InitializeLevels(List<string> levelNames)
    {
        if (Levels.Count == levelNames.Count)
            return;
        
        Levels.Clear();
        for (int i = 0; i < levelNames.Count; i++)
        {
            Levels.Add(new LevelProgress(i, levelNames[i]));
        }
    }

    public int GetNextUncompletedLevelIndex()
    {
        for (int i = 0; i < Levels.Count; i++)
        {
            if (!Levels[i].IsCompleted)
                return i;
        }
        // Если все пройдены - возвращаем последний
        return Math.Max(0, Levels.Count - 1);
    }

    public bool CanPlayLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= Levels.Count)
            return false;
        
        // Первый уровень 
        if (levelIndex == 0)
            return true;
        
        // Остальные уровни доступны если пройден предыдущий или это первый непройденный
        return Levels[levelIndex - 1].IsCompleted || levelIndex == GetNextUncompletedLevelIndex();
    }

    public void SaveProgress()
    {
        try
        {
            var filePath = GetProgressFilePath();
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error saving progress: {ex.Message}");
        }
    }

    public static PlayerProgress LoadProgress()
    {
        try
        {
            var filePath = GetProgressFilePath();
            if (!File.Exists(filePath))
                return new PlayerProgress();
            
            var json = File.ReadAllText(filePath);
            var progress = JsonSerializer.Deserialize<PlayerProgress>(json);
            return progress ?? new PlayerProgress();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading progress: {ex.Message}");
            return new PlayerProgress();
        }
    }
}
