namespace NumberGuessingGame;

public static class InputHelper
{
    public static int ReadInt(string message, int min, int max)
    {
        while (true)
        {
            Console.Write(message);
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int value))
            {
                Console.WriteLine("Invalid input, please enter a number.");
                continue;
            }

            if (value < min || value > max)
            {
                Console.WriteLine($"Please enter a number between {min} and {max}.");
                continue;
            }

            return value;
        }
    }

    public static bool AskYesNo(string message)
    {
        while (true)
        {
            Console.Write($"{message} (y/n): ");
            string? answer = Console.ReadLine()?.Trim().ToLower();

            if (answer == "y") return true;
            if (answer == "n") return false;

            Console.WriteLine("Please type 'y' or 'n'.");
        }
    }
}