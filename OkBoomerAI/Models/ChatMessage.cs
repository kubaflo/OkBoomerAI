using CommunityToolkit.Mvvm.ComponentModel;

namespace OkBoomerAI.Models;

public partial class ChatMessage : ObservableObject
{
    [ObservableProperty]
    private string _content = string.Empty;

    [ObservableProperty]
    private bool _isUser;

    [ObservableProperty]
    private bool _isStreaming;

    public DateTime Timestamp { get; set; } = DateTime.Now;
}
