Console.WriteLine("== Number Guessing Game ==");
Console.WriteLine("I'm thinking of a number between 1 and 100.");
Console.WriteLine();

var random = new Random();
int secretNumber = random.Next(1,101);
int attempts = 0;

while (true)
{
    Console.WriteLine("Your Guess: ");
    string? input = Console.ReadLine();
    if(!int.TryParse(input, out int guess))
    {
        Console.WriteLine("Please Enter Valid Number");
        continue;
    }
    attempts++;
    if(guess < secretNumber)
    {
        Console.WriteLine("Too low! Try a bigger number.");
    }
    else if (guess > secretNumber)
    {
        Console.WriteLine("Too high! Try a smaller number.");

    }
    else
    {
        Console.WriteLine($"Correct! The number was {secretNumber}.");
        Console.WriteLine($"You needed {attempts} attempt(s).");
        if(guess <= 5)
        {
            Console.WriteLine("Excellent, you guessed it in fewer than 5 attempts.");
        }
        break;
    }

}