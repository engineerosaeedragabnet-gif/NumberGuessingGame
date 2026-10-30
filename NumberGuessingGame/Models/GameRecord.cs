namespace NumberGuessingGame.Models;

public record GameRecord(
    string PlayerName,
    DifficultyLevel Level,
    int Attempts,
    int Score,
    DateTime PlayedAt);