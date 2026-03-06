using System.Text.Json.Serialization;
namespace OkBoomerAI.Models;
public class JokeExplanation
{
    [JsonPropertyName("category")] public string Category { get; set; } = string.Empty;
    [JsonPropertyName("confusion_stars")] public int ConfusionStars { get; set; }
    [JsonPropertyName("explanation")] public string Explanation { get; set; } = string.Empty;
    [JsonPropertyName("humor_note")] public string HumorNote { get; set; } = string.Empty;
}
