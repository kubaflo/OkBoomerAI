using System.Text.Json.Serialization;

namespace OkBoomerAI.Models;

public class VibeResult
{
    [JsonPropertyName("vibe")]
    public string Vibe { get; set; } = string.Empty;

    [JsonPropertyName("confidence")]
    public double Confidence { get; set; }

    [JsonPropertyName("emoji")]
    public string Emoji { get; set; } = string.Empty;

    [JsonPropertyName("explanation")]
    public string Explanation { get; set; } = string.Empty;

    [JsonPropertyName("boomer_translation")]
    public string BoomerTranslation { get; set; } = string.Empty;
}
