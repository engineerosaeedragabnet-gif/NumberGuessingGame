using NumberGuessingGame;

Console.WriteLine("=== Number Guessing Game ===");

int totalScore = 0;
bool playAgain;

do
{
    DifficultySettings settings = ChooseDifficulty();
    int roundScore = PlayRound(settings);
    totalScore += roundScore;

    Console.WriteLine($"Round score: {roundScore} | Total score: {totalScore}");
    Console.WriteLine();

    playAgain = InputHelper.AskYesNo("Play again?");
}
while (playAgain);

Console.WriteLine($"Thanks for playing! Final score: {totalScore}");

DifficultySettings ChooseDifficulty()
{
    Console.WriteLine();
    Console.WriteLine("Choose difficulty:");
    Console.WriteLine("1. Easy   (1-50,  10 attempts)");
    Console.WriteLine("2. Medium (1-100, 7 attempts)");
    Console.WriteLine("3. Hard   (1-200, 7 attempts)");

    int choice = InputHelper.ReadInt("Your choice: ", 1, 3);

    return choice switch
    {
        1 => DifficultySettings.Easy,
        2 => DifficultySettings.Medium,
        _ => DifficultySettings.Hard
    };
}

int PlayRound(DifficultySettings settings)
{
    var game = new GuessingGame(settings);

    Console.WriteLine();
    Console.WriteLine($"I'm thinking of a number between {settings.Min} and {settings.Max}.");
    Console.WriteLine($"You have {settings.MaxAttempts} attempts. Good luck!");

    while (!game.IsOver)
    {
        Console.WriteLine();
        int guess = InputHelper.ReadInt(
            $"Attempt {game.Attempts + 1}/{settings.MaxAttempts} - your guess: ",
            settings.Min, settings.Max);

        GuessResult result = game.MakeGuess(guess);

        switch (result)
        {
            case GuessResult.TooLow:
                Console.WriteLine("Too low!");
                break;
            case GuessResult.TooHigh:
                Console.WriteLine("Too high!");
                break;
            case GuessResult.Correct:
                Console.WriteLine($"Correct! You won in {game.Attempts} attempt(s).");
                break;
        }
    }

    if (!game.IsWon)
    {
        Console.WriteLine($"Game over! The number was {game.RevealedNumber}.");
    }

    return game.CalculateScore();
}