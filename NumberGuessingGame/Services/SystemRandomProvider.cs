namespace NumberGuessingGame.Services;

public class SystemRandomProvider : IRandomProvider
{
    private readonly Random _random = new();

    public int Next(int minInclusive, int maxInclusive)
        => _random.Next(minInclusive, maxInclusive + 1);
}