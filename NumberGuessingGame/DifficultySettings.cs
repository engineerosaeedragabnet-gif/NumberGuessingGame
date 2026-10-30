namespace NumberGuessingGame;

public record DifficultySettings(string Name, int Min, int Max, int MaxAttempts)
{
    public static readonly DifficultySettings Easy = new("Easy", 1, 50, 10);
    public static readonly DifficultySettings Medium = new("Medium", 1, 100, 7);
    public static readonly DifficultySettings Hard = new("Hard", 1, 200, 7);
}