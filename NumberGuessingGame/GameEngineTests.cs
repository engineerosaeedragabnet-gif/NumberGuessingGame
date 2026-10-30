using NumberGuessingGame.Models;
using NumberGuessingGame.Services;
using Xunit;

namespace NumberGuessingGame.Tests;

// Random مزيّف بيرجّع رقم ثابت، فنقدر نتوقع النتيجة بالظبط
public class FakeRandomProvider : IRandomProvider
{
    private readonly int _value;
    public FakeRandomProvider(int value) => _value = value;
    public int Next(int minInclusive, int maxInclusive) => _value;
}

public class GameEngineTests
{
    private static GameEngine CreateEngine(int secret, DifficultyLevel level = DifficultyLevel.Medium)
        => new(DifficultySettings.From(level), new FakeRandomProvider(secret));

    [Fact]
    public void CorrectGuess_WinsTheGame()
    {
        var engine = CreateEngine(secret: 42);

        var outcome = engine.MakeGuess(42);

        Assert.Equal(GuessResult.Correct, outcome.Result);
        Assert.Equal(GameState.Won, engine.State);
    }

    [Theory]
    [InlineData(10, GuessResult.TooLow)]
    [InlineData(90, GuessResult.TooHigh)]
    public void WrongGuess_ReturnsCorrectHint(int guess, GuessResult expected)
    {
        var engine = CreateEngine(secret: 50);

        Assert.Equal(expected, engine.MakeGuess(guess).Result);
    }

    [Fact]
    public void DuplicateGuess_DoesNotConsumeAttempt()
    {
        var engine = CreateEngine(secret: 50);

        engine.MakeGuess(10);
        var outcome = engine.MakeGuess(10);

        Assert.Equal(GuessResult.AlreadyGuessed, outcome.Result);
        Assert.Equal(1, engine.Attempts);
    }

    [Fact]
    public void OutOfRangeGuess_DoesNotConsumeAttempt()
    {
        var engine = CreateEngine(secret: 50);

        var outcome = engine.MakeGuess(500);

        Assert.Equal(GuessResult.OutOfRange, outcome.Result);
        Assert.Equal(0, engine.Attempts);
    }

    [Fact]
    public void RunningOutOfAttempts_LosesTheGame()
    {
        var engine = CreateEngine(secret: 100); // Medium = 7 محاولات

        for (int i = 1; i <= 7; i++)
            engine.MakeGuess(i);

        Assert.Equal(GameState.Lost, engine.State);
        Assert.Equal(0, engine.CalculateScore());
    }

    [Fact]
    public void GuessingAfterGameOver_Throws()
    {
        var engine = CreateEngine(secret: 42);
        engine.MakeGuess(42);

        Assert.Throws<InvalidOperationException>(() => engine.MakeGuess(1));
    }

    [Fact]
    public void Score_UsesDifficultyMultiplier()
    {
        var engine = CreateEngine(secret: 42, DifficultyLevel.Hard);
        engine.MakeGuess(42); // أول محاولة: (6+1) * 10 * 3

        Assert.Equal(210, engine.CalculateScore());
    }
}