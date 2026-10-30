namespace NumberGuessingGame.Models;

public enum GuessResult
{
    TooLow,
    TooHigh,
    Correct,
    AlreadyGuessed, // نفس الرقم اتخمن قبل كده (مش بيستهلك محاولة)
    OutOfRange      // الرقم خارج النطاق (مش بيستهلك محاولة)
}

public enum Temperature
{
    None,
    Cold,
    Warm,
    Hot
}

public enum GameState
{
    InProgress,
    Won,
    Lost
}

public record GuessOutcome(GuessResult Result, Temperature Temperature);