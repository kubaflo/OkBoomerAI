using System.Text.Json.Serialization;

namespace OkBoomerAI.Models;

public class QuizQuestion
{
    [JsonPropertyName("question")]
    public string Question { get; set; } = string.Empty;

    [JsonPropertyName("options")]
    public List<string> Options { get; set; } = [];

    [JsonPropertyName("correct_index")]
    public int CorrectIndex { get; set; }

    [JsonPropertyName("explanation")]
    public string Explanation { get; set; } = string.Empty;

    [JsonPropertyName("roast")]
    public string Roast { get; set; } = string.Empty;
}
