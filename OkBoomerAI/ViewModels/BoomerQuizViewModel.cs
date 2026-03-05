using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OkBoomerAI.Models;
using OkBoomerAI.Services;

namespace OkBoomerAI.ViewModels;

public partial class BoomerQuizViewModel : ObservableObject
{
    private readonly IChatService _chatService;

    [ObservableProperty]
    private string _question = string.Empty;

    [ObservableProperty]
    private List<string> _options = [];

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private bool _hasQuestion;

    [ObservableProperty]
    private bool _hasAnswered;

    [ObservableProperty]
    private bool _isCorrect;

    [ObservableProperty]
    private string _feedback = string.Empty;

    [ObservableProperty]
    private int _score;

    [ObservableProperty]
    private int _streak;

    [ObservableProperty]
    private int _totalQuestions;

    [ObservableProperty]
    private int _selectedIndex = -1;

    private QuizQuestion? _currentQuestion;

    private const string QuizSchema = """
        {
            "type": "object",
            "properties": {
                "question": { "type": "string" },
                "options": { "type": "array", "items": { "type": "string" } },
                "correct_index": { "type": "integer" },
                "explanation": { "type": "string" },
                "roast": { "type": "string" }
            },
            "required": ["question", "options", "correct_index", "explanation", "roast"]
        }
        """;

    public BoomerQuizViewModel(IChatService chatService)
    {
        _chatService = chatService;
    }

    [RelayCommand]
    private async Task GenerateQuestion()
    {
        IsBusy = true;
        HasAnswered = false;
        HasQuestion = false;
        SelectedIndex = -1;
        Feedback = string.Empty;

        try
        {
            var previousContext = TotalQuestions > 0
                ? $"This is question #{TotalQuestions + 1}. The player's score is {Score}/{TotalQuestions}. Generate a new unique question."
                : "Generate the first question.";

            var json = await _chatService.GetStructuredResponseAsync(
                Prompts.QuizGenerator, previousContext, QuizSchema);

            _currentQuestion = System.Text.Json.JsonSerializer.Deserialize<QuizQuestion>(json);
            if (_currentQuestion != null)
            {
                Question = _currentQuestion.Question;
                Options = _currentQuestion.Options;
                HasQuestion = true;
            }
        }
        catch (Exception ex)
        {
            Question = $"Quiz generation failed: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void SubmitAnswer(int index)
    {
        if (_currentQuestion == null || HasAnswered) return;

        SelectedIndex = index;
        HasAnswered = true;
        TotalQuestions++;

        IsCorrect = index == _currentQuestion.CorrectIndex;

        if (IsCorrect)
        {
            Score++;
            Streak++;
            Feedback = $"✅ Correct! {_currentQuestion.Explanation}";
            if (Streak > 2)
                Feedback += $"\n🔥 {Streak} streak! You might not be a total boomer after all.";
        }
        else
        {
            Streak = 0;
            Feedback = $"❌ Wrong! {_currentQuestion.Roast}\n\n📖 {_currentQuestion.Explanation}";
        }
    }

    [RelayCommand]
    private void ResetScore()
    {
        Score = 0;
        Streak = 0;
        TotalQuestions = 0;
        HasQuestion = false;
        HasAnswered = false;
    }
}
