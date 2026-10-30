using NumberGuessingGame.Models;
using NumberGuessingGame.Services;

namespace NumberGuessingGame.UI;

public class ConsoleUI
{
    private readonly IRandomProvider _randomProvider;
    private readonly IScoreRepository _scoreRepository;

    // بنستقبل الـ Dependencies من بره (Dependency Injection)
    public ConsoleUI(IRandomProvider randomProvider, IScoreRepository scoreRepository)
    {
        _randomProvider = randomProvider;
        _scoreRepository = scoreRepository;
    }

    public void Run()
    {
        Console.WriteLine("=== Number Guessing Game ===");
        string playerName = ReadName();

        bool running = true;
        while (running)
        {
            Console.WriteLine();
            Console.WriteLine("1. Play");
            Console.WriteLine("2. High Scores");
            Console.WriteLine("0. Exit");

            switch (ReadInt("Choose: ", 0, 2))
            {
                case 1: PlayRound(playerName); break;
                case 2: ShowHighScores(); break;
                case 0:
                    running = false;
                    Console.WriteLine("Goodbye!");
                    break;
            }
        }
    }

    private void PlayRound(string playerName)
    {
        DifficultySettings settings = DifficultySettings.From(ChooseDifficulty());
        var engine = new GameEngine(settings, _randomProvider);

        Console.WriteLine();
        Console.WriteLine($"Guess the number between {settings.Min} and {settings.Max}.");
        Console.WriteLine($"You have {settings.MaxAttempts} attempts.");

        while (engine.State == GameState.InProgress)
        {
            Console.WriteLine();
            int guess = ReadInt($"Attempt {engine.Attempts + 1}/{settings.MaxAttempts}: ",
                int.MinValue, int.MaxValue);

            GuessOutcome outcome = engine.MakeGuess(guess);
            PrintOutcome(outcome, engine);
        }

        PrintSummary(engine, playerName);
    }

    private void PrintOutcome(GuessOutcome outcome, GameEngine engine)
    {
        switch (outcome.Result)
        {
            case GuessResult.OutOfRange:
                Console.WriteLine($"Out of range! Use {engine.Settings.Min}-{engine.Settings.Max}.");
                break;
            case GuessResult.AlreadyGuessed:
                Console.WriteLine("You already tried that number.");
                break;
            case GuessResult.Correct:
                Console.WriteLine("Correct!");
                break;
            case GuessResult.TooLow:
                Console.WriteLine($"Too low! {TemperatureText(outcome.Temperature)}");
                break;
            case GuessResult.TooHigh:
                Console.WriteLine($"Too high! {TemperatureText(outcome.Temperature)}");
                break;
        }

        if (engine.Guesses.Count > 0 && engine.State == GameState.InProgress)
        {
            Console.WriteLine($"Previous guesses: {string.Join(", ", engine.Guesses)}");
        }
    }

    private void PrintSummary(GameEngine engine, string playerName)
    {
        Console.WriteLine();

        if (engine.State == GameState.Won)
        {
            int score = engine.CalculateScore();
            Console.WriteLine($"You won in {engine.Attempts} attempt(s)! Score: {score}");

            _scoreRepository.Save(new GameRecord(
                playerName, engine.Settings.Level, engine.Attempts, score, DateTime.Now));
        }
        else
        {
            Console.WriteLine($"Game over! The number was {engine.RevealedNumber}.");
        }
    }

    private void ShowHighScores()
    {
        IReadOnlyList<GameRecord> top = _scoreRepository.GetTop(5);

        Console.WriteLine();
        Console.WriteLine("---------- High Scores ----------");

        if (top.Count == 0)
        {
            Console.WriteLine("No scores yet. Be the first!");
            return;
        }

        for (int i = 0; i < top.Count; i++)
        {
            GameRecord r = top[i];
            Console.WriteLine($"{i + 1}. {r.PlayerName,-12} {r.Score,4} pts | {r.Level,-6} | {r.Attempts} attempts");
        }
    }

    private DifficultyLevel ChooseDifficulty()
    {
        Console.WriteLine();
        Console.WriteLine("1. Easy   (1-50,  10 attempts, x1)");
        Console.WriteLine("2. Medium (1-100, 7 attempts,  x2)");
        Console.WriteLine("3. Hard   (1-200, 7 attempts,  x3)");

        return (DifficultyLevel)ReadInt("Difficulty: ", 1, 3);
    }

    private static string TemperatureText(Temperature t) => t switch
    {
        Temperature.Hot => "🔥 Very close!",
        Temperature.Warm => "🙂 Getting warmer.",
        Temperature.Cold => "❄️ Cold.",
        _ => string.Empty
    };

    private static string ReadName()
    {
        Console.Write("Enter your name: ");
        string? name = Console.ReadLine()?.Trim();
        return string.IsNullOrWhiteSpace(name) ? "Player" : name;
    }

    private static int ReadInt(string message, int min, int max)
    {
        while (true)
        {
            Console.Write(message);

            if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
                return value;

            Console.WriteLine("Invalid input, please try again.");
        }
    }
}