using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OkBoomerAI.Models;
using OkBoomerAI.Services;

namespace OkBoomerAI.ViewModels;

public partial class VibeCheckViewModel : ObservableObject
{
    private readonly IChatService _chatService;

    [ObservableProperty]
    private string _inputText = string.Empty;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private bool _hasResult;

    [ObservableProperty]
    private string _vibeEmoji = string.Empty;

    [ObservableProperty]
    private string _vibeName = string.Empty;

    [ObservableProperty]
    private double _confidence;

    [ObservableProperty]
    private string _explanation = string.Empty;

    [ObservableProperty]
    private string _boomerTranslation = string.Empty;

    [ObservableProperty]
    private Color _vibeColor = Colors.Gray;

    private const string VibeSchema = """
        {
            "type": "object",
            "properties": {
                "vibe": { "type": "string" },
                "confidence": { "type": "number" },
                "emoji": { "type": "string" },
                "explanation": { "type": "string" },
                "boomer_translation": { "type": "string" }
            },
            "required": ["vibe", "confidence", "emoji", "explanation", "boomer_translation"]
        }
        """;

    private static readonly Dictionary<string, Color> VibeColors = new()
    {
        ["sarcastic"] = Color.FromArgb("#FF6B6B"),
        ["wholesome"] = Color.FromArgb("#51CF66"),
        ["toxic"] = Color.FromArgb("#FF0044"),
        ["flirty"] = Color.FromArgb("#FF69B4"),
        ["passive_aggressive"] = Color.FromArgb("#FFA500"),
        ["chaotic"] = Color.FromArgb("#9B59B6"),
        ["cringe"] = Color.FromArgb("#E67E22"),
        ["sus"] = Color.FromArgb("#F1C40F"),
        ["slay"] = Color.FromArgb("#00D2FF"),
        ["mid"] = Color.FromArgb("#95A5A6"),
    };

    public VibeCheckViewModel(IChatService chatService)
    {
        _chatService = chatService;
    }

    [RelayCommand]
    private async Task AnalyzeVibe()
    {
        var text = InputText?.Trim();
        if (string.IsNullOrEmpty(text)) return;

        IsBusy = true;
        HasResult = false;

        try
        {
            var json = await _chatService.GetStructuredResponseAsync(
                Prompts.VibeCheck, text, VibeSchema);

            var result = System.Text.Json.JsonSerializer.Deserialize<VibeResult>(json);
            if (result != null)
            {
                VibeEmoji = result.Emoji;
                VibeName = result.Vibe.ToUpperInvariant();
                Confidence = result.Confidence;
                Explanation = result.Explanation;
                BoomerTranslation = result.BoomerTranslation;
                VibeColor = VibeColors.GetValueOrDefault(result.Vibe, Colors.Gray);
                HasResult = true;
            }
        }
        catch (Exception ex)
        {
            VibeName = "ERROR";
            VibeEmoji = "💥";
            Explanation = $"Vibe check failed: {ex.Message}";
            HasResult = true;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void Reset()
    {
        InputText = string.Empty;
        HasResult = false;
    }
}
