using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OkBoomerAI.Services;

namespace OkBoomerAI.ViewModels;

public partial class SlangDecoderViewModel : ObservableObject
{
    private readonly IChatService _chatService;

    private static readonly string ResponseJsonSchema = """
        {
            "type": "object",
            "properties": {
                "category": { "type": "string" },
                "confusion_stars": { "type": "integer" },
                "explanation": { "type": "string" },
                "humor_note": { "type": "string" }
            },
            "required": ["category", "confusion_stars", "explanation", "humor_note"]
        }
        """;

    [ObservableProperty]
    private string _inputText = string.Empty;

    [ObservableProperty]
    private double _confusionLevel = 0.5;

    [ObservableProperty]
    private bool _hasResult;

    [ObservableProperty]
    private string _category = string.Empty;

    [ObservableProperty]
    private int _confusionStars;

    [ObservableProperty]
    private string _explanationText = string.Empty;

    [ObservableProperty]
    private string _humorNote = string.Empty;

    [ObservableProperty]
    private bool _isBusy;

    private CancellationTokenSource? _cts;

    public SlangDecoderViewModel(IChatService chatService)
    {
        _chatService = chatService;
    }

    [RelayCommand]
    private async Task Explain()
    {
        var text = InputText?.Trim();
        if (string.IsNullOrEmpty(text)) return;

        await RequestExplanationAsync(text, simpler: false);
    }

    [RelayCommand]
    private async Task Huh()
    {
        var text = InputText?.Trim();
        if (string.IsNullOrEmpty(text) || !HasResult) return;

        await RequestExplanationAsync(text, simpler: true);
    }

    [RelayCommand]
    private async Task Share()
    {
        if (!HasResult || string.IsNullOrEmpty(ExplanationText)) return;

        var shareText = $"[{Category}] ⭐ {ConfusionStars}/5\n\n{ExplanationText}\n\n😂 {HumorNote}";
        await Clipboard.Default.SetTextAsync(shareText);
    }

    [RelayCommand]
    private void Reset()
    {
        _cts?.Cancel();
        InputText = string.Empty;
        ConfusionLevel = 0.5;
        HasResult = false;
        Category = string.Empty;
        ConfusionStars = 0;
        ExplanationText = string.Empty;
        HumorNote = string.Empty;
        IsBusy = false;
    }

    private async Task RequestExplanationAsync(string text, bool simpler)
    {
        IsBusy = true;

        try
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            var confusionLabel = ConfusionLevel switch
            {
                < 0.25 => "mildly confused (they're young-ish)",
                < 0.5 => "moderately confused",
                < 0.75 => "very confused",
                _ => "full boomer, completely clueless"
            };

            var userMessage = simpler
                ? $"Explain even simpler, I really don't get it: \"{text}\" (confusion level: {confusionLabel})"
                : $"Explain this: \"{text}\" (confusion level: {confusionLabel})";

            var json = await _chatService.GetStructuredResponseAsync(
                Prompts.SlangDecoderStructured, userMessage, ResponseJsonSchema, _cts.Token);

            var result = JsonSerializer.Deserialize<SlangDecoderResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result is not null)
            {
                Category = result.Category ?? "Unknown";
                ConfusionStars = Math.Clamp(result.ConfusionStars, 1, 5);
                ExplanationText = result.Explanation ?? string.Empty;
                HumorNote = result.HumorNote ?? string.Empty;
                HasResult = true;
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            ExplanationText = $"Bruh, something went wrong 💀: {ex.Message}";
            Category = "Error";
            ConfusionStars = 5;
            HumorNote = "Even the AI is confused";
            HasResult = true;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private sealed class SlangDecoderResponse
    {
        public string? Category { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("confusion_stars")]
        public int ConfusionStars { get; set; }

        public string? Explanation { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("humor_note")]
        public string? HumorNote { get; set; }
    }
}
