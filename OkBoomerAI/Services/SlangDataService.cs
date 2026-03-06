using System.Reflection;
using System.Text.Json;
using OkBoomerAI.Models;

namespace OkBoomerAI.Services;

public class SlangDataService
{
    private List<SlangEntry>? _entries;

    public async Task<List<SlangEntry>> GetEntriesAsync()
    {
        if (_entries != null) return _entries;
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("slang_dictionary.json");
            if (stream != null)
            {
                _entries = await JsonSerializer.DeserializeAsync<List<SlangEntry>>(stream) ?? [];
            }
            else
            {
                _entries = [];
            }
        }
        catch
        {
            _entries = [];
        }
        return _entries;
    }
}
