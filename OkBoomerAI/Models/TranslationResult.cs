using System.Text.Json.Serialization;

namespace OkBoomerAI.Models;

public class TranslationResult
{
    [JsonPropertyName("translation")]
    public string Translation { get; set; } = string.Empty;

    [JsonPropertyName("tone_shift")]
    public string ToneShift { get; set; } = string.Empty;

    [JsonPropertyName("notes")]
    public string Notes { get; set; } = string.Empty;
}
