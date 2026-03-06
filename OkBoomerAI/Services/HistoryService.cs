namespace OkBoomerAI.Services;

public class HistoryService
{
    private readonly List<HistoryEntry> _entries = [];

    public void Add(string input, string explanation, string category)
    {
        _entries.Insert(0, new HistoryEntry
        {
            Input = input,
            Explanation = explanation,
            Category = category,
            Timestamp = DateTime.Now
        });

        if (_entries.Count > 50)
            _entries.RemoveAt(_entries.Count - 1);
    }

    public IReadOnlyList<HistoryEntry> GetAll() => _entries.AsReadOnly();

    public void Clear() => _entries.Clear();
}

public class HistoryEntry
{
    public string Input { get; set; } = "";
    public string Explanation { get; set; } = "";
    public string Category { get; set; } = "";
    public DateTime Timestamp { get; set; }
}
