using NumberGuessingGame.Models;

namespace NumberGuessingGame.Services;

public interface IScoreRepository
{
    void Save(GameRecord record);
    IReadOnlyList<GameRecord> GetTop(int count);
}