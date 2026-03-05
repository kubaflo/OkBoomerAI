using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OkBoomerAI.Models;
using OkBoomerAI.Services;

namespace OkBoomerAI.ViewModels;

public partial class SlangDecoderViewModel : ObservableObject
{
    private readonly IChatService _chatService;

    [ObservableProperty]
    private string _inputText = string.Empty;

    [ObservableProperty]
    private bool _isBusy;

    public ObservableCollection<ChatMessage> Messages { get; } = [];

    private CancellationTokenSource? _cts;

    public SlangDecoderViewModel(IChatService chatService)
    {
        _chatService = chatService;
    }

    [RelayCommand]
    private async Task SendMessage()
    {
        var text = InputText?.Trim();
        if (string.IsNullOrEmpty(text)) return;

        Messages.Add(new ChatMessage { Content = text, IsUser = true });
        InputText = string.Empty;
        IsBusy = true;

        var aiMessage = new ChatMessage { Content = "", IsUser = false, IsStreaming = true };
        Messages.Add(aiMessage);

        try
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            var history = Messages.ToList();

            await foreach (var chunk in _chatService.GetStreamingResponseAsync(
                Prompts.SlangDecoder, history, _cts.Token))
            {
                aiMessage.Content += chunk;
                OnPropertyChanged(nameof(Messages));
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            aiMessage.Content = $"Bruh, something went wrong 💀: {ex.Message}";
        }
        finally
        {
            aiMessage.IsStreaming = false;
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void ClearChat()
    {
        _cts?.Cancel();
        Messages.Clear();
    }
}
