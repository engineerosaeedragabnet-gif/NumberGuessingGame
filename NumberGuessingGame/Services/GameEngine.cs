using NumberGuessingGame.Models;

namespace NumberGuessingGame.Services;

public class GameEngine
{
    private readonly int _secretNumber;
    private readonly List<int> _guesses = new();

    public DifficultySettings Settings { get; }
    public GameState State { get; private set; } = GameState.InProgress;

    public IReadOnlyList<int> Guesses => _guesses;
    public int Attempts => _guesses.Count;
    public int RemainingAttempts => Settings.MaxAttempts - Attempts;

    public int? RevealedNumber => State == GameState.InProgress ? null : _secretNumber;

    public GameEngine(DifficultySettings settings, IRandomProvider randomProvider)
    {
        Settings = settings;
        _secretNumber = randomProvider.Next(settings.Min, settings.Max);
    }

    public GuessOutcome MakeGuess(int guess)
    {
        if (State != GameState.InProgress)
            throw new InvalidOperationException("The game is already over.");

        if (guess < Settings.Min || guess > Settings.Max)
            return new GuessOutcome(GuessResult.OutOfRange, Temperature.None);

        if (_guesses.Contains(guess))
            return new GuessOutcome(GuessResult.AlreadyGuessed, Temperature.None);

        _guesses.Add(guess);

        if (guess == _secretNumber)
        {
            State = GameState.Won;
            return new GuessOutcome(GuessResult.Correct, Temperature.Hot);
        }

        if (RemainingAttempts <= 0)
            State = GameState.Lost;

        GuessResult result = guess < _secretNumber ? GuessResult.TooLow : GuessResult.TooHigh;
        return new GuessOutcome(result, GetTemperature(guess));
    }

    public int CalculateScore()
    {
        if (State != GameState.Won) return 0;

        return (RemainingAttempts + 1) * 10 * Settings.ScoreMultiplier;
    }

    // القرب بيتحسب كنسبة من حجم النطاق
    private Temperature GetTemperature(int guess)
    {
        double distanceRatio = (double)Math.Abs(guess - _secretNumber) / Settings.Range;

        return distanceRatio switch
        {
            <= 0.05 => Temperature.Hot,
            <= 0.15 => Temperature.Warm,
            _ => Temperature.Cold
        };
    }
}