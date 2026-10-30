namespace NumberGuessingGame.Models;

public enum DifficultyLevel
{
    Easy = 1,
    Medium = 2,
    Hard = 3
}

public record DifficultySettings(DifficultyLevel Level, int Min, int Max, int MaxAttempts, int ScoreMultiplier)
{
    public int Range => Max - Min;

    public static DifficultySettings From(DifficultyLevel level) => level switch
    {
        DifficultyLevel.Easy => new(level, 1, 50, 10, 1),
        DifficultyLevel.Medium => new(level, 1, 100, 7, 2),
        DifficultyLevel.Hard => new(level, 1, 200, 7, 3),
        _ => throw new ArgumentOutOfRangeException(nameof(level))
    };
}