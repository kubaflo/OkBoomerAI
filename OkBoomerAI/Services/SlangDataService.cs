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
}
