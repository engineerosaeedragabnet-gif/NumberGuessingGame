namespace NumberGuessingGame;

public enum GuessResult
{
    TooLow,
    TooHigh,
    Correct
}

public class GuessingGame
{
    private readonly Random _random = new();
    private readonly int _secretNumber;

    public DifficultySettings Settings { get; }
    public int Attempts { get; private set; }
    public bool IsWon { get; private set; }

    public int RemainingAttempts => Settings.MaxAttempts - Attempts;
    public bool IsOver => IsWon || RemainingAttempts <= 0;

    public int? RevealedNumber => IsOver ? _secretNumber : null;

    public GuessingGame(DifficultySettings settings)
    {
        Settings = settings;
        _secretNumber = _random.Next(settings.Min, settings.Max + 1);
    }

    public GuessResult MakeGuess(int guess)
    {
        if (IsOver)
            throw new InvalidOperationException("The game is already over.");

        Attempts++;

        if (guess == _secretNumber)
        {
            IsWon = true;
            return GuessResult.Correct;
        }

        return guess < _secretNumber ? GuessResult.TooLow : GuessResult.TooHigh;
    }

    public int CalculateScore()
    {
        return IsWon ? (RemainingAttempts + 1) * 10 : 0;
    }
}