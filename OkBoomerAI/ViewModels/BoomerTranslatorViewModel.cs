using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OkBoomerAI.Services;

namespace OkBoomerAI.ViewModels;

public partial class BoomerTranslatorViewModel : ObservableObject
{
    private readonly IChatService _chatService;

    [ObservableProperty]
    private string _inputText = string.Empty;

    [ObservableProperty]
    private string _translatedText = string.Empty;

    [ObservableProperty]
    private string _toneShift = string.Empty;

    [ObservableProperty]
    private string _notes = string.Empty;

    [ObservableProperty]
    private bool _isBoomerToGenZ = true;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _directionLabel = "Boomer → Gen-Z";

    private const string TranslationSchema = """
        {
            "type": "object",
            "properties": {
                "translation": { "type": "string" },
                "tone_shift": { "type": "string" },
                "notes": { "type": "string" }
            },
            "required": ["translation", "tone_shift", "notes"]
        }
        """;

    public BoomerTranslatorViewModel(IChatService chatService)
    {
        _chatService = chatService;
    }

    [RelayCommand]
    private void ToggleDirection()
    {
        IsBoomerToGenZ = !IsBoomerToGenZ;
        DirectionLabel = IsBoomerToGenZ ? "Boomer → Gen-Z" : "Gen-Z → Boomer";
        TranslatedText = string.Empty;
        ToneShift = string.Empty;
        Notes = string.Empty;
    }

    [RelayCommand]
    private async Task Translate()
    {
        var text = InputText?.Trim();
        if (string.IsNullOrEmpty(text)) return;

        IsBusy = true;
        TranslatedText = string.Empty;

        try
        {
            var prompt = IsBoomerToGenZ ? Prompts.BoomerToGenZ : Prompts.GenZToBoomer;
            var json = await _chatService.GetStructuredResponseAsync(prompt, text, TranslationSchema);

            var result = System.Text.Json.JsonSerializer.Deserialize<Models.TranslationResult>(json);
            if (result != null)
            {
                TranslatedText = result.Translation;
                ToneShift = result.ToneShift;
                Notes = result.Notes;
            }
        }
        catch (Exception ex)
        {
            TranslatedText = $"Translation failed: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
