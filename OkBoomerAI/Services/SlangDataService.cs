using System.Text.Json;
using OkBoomerAI.Models;

namespace OkBoomerAI.Services;

public class SlangDataService
{
    private List<SlangEntry>? _entries;

    public async Task<List<SlangEntry>> GetEntriesAsync()
    {
        if (_entries != null) return _entries;

        using var stream = await FileSystem.OpenAppPackageFileAsync("slang_dictionary.json");
        _entries = await JsonSerializer.DeserializeAsync<List<SlangEntry>>(stream) ?? [];
        return _entries;
    }

    public async Task<List<SlangEntry>> SearchAsync(string query)
    {
        var entries = await GetEntriesAsync();
        var q = query.ToLowerInvariant();
        return entries
            .Where(e => e.Term.Contains(q, StringComparison.OrdinalIgnoreCase)
                     || e.Definition.Contains(q, StringComparison.OrdinalIgnoreCase)
                     || e.Category.Contains(q, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}
