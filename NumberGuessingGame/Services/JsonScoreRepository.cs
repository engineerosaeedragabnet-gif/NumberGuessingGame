using System.Text.Json;
using System.Text.Json.Serialization;
using NumberGuessingGame.Models;

namespace NumberGuessingGame.Services;

public class JsonScoreRepository : IScoreRepository
{
    private readonly string _filePath;

    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() } // يكتب "Easy" بدل 1
    };

    public JsonScoreRepository(string filePath = "highscores.json")
    {
        _filePath = filePath;
    }

    public void Save(GameRecord record)
    {
        List<GameRecord> records = Load();
        records.Add(record);

        string json = JsonSerializer.Serialize(records, Options);
        File.WriteAllText(_filePath, json);
    }

    public IReadOnlyList<GameRecord> GetTop(int count)
    {
        return Load()
            .OrderByDescending(r => r.Score)
            .ThenBy(r => r.Attempts)
            .Take(count)
            .ToList();
    }

    private List<GameRecord> Load()
    {
        if (!File.Exists(_filePath))
            return new List<GameRecord>();

        try
        {
            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<GameRecord>>(json, Options)
                   ?? new List<GameRecord>();
        }
        catch (JsonException)
        {
            // لو الملف بايظ منقعش البرنامج، بنبدأ من جديد
            return new List<GameRecord>();
        }
    }
}