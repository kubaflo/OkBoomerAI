using System.Runtime.CompilerServices;
using Microsoft.Extensions.AI;

namespace OkBoomerAI.Services;

public class AppleIntelligenceChatService : IChatService
{
    private readonly IChatClient _chatClient;

    public AppleIntelligenceChatService(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task<string> GetResponseAsync(string systemPrompt, string userMessage, CancellationToken ct = default)
    {
        var messages = new List<ChatMessage>
        {
            new(ChatRole.System, systemPrompt),
            new(ChatRole.User, userMessage)
        };
        var response = await _chatClient.GetResponseAsync(messages, cancellationToken: ct);
        return response.Text ?? string.Empty;
    }

    public async IAsyncEnumerable<string> GetStreamingResponseAsync(
        string systemPrompt, string userMessage,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        var messages = new List<ChatMessage>
        {
            new(ChatRole.System, systemPrompt),
            new(ChatRole.User, userMessage)
        };
        await foreach (var update in _chatClient.GetStreamingResponseAsync(messages, cancellationToken: ct))
        {
            if (!string.IsNullOrEmpty(update.Text))
                yield return update.Text;
        }
    }

    public async Task<string> GetStructuredResponseAsync(
        string systemPrompt, string userMessage, string jsonSchema, CancellationToken ct = default)
    {
        var messages = new List<ChatMessage>
        {
            new(ChatRole.System, systemPrompt + "\n\nRespond with ONLY valid JSON matching this schema:\n" + jsonSchema),
            new(ChatRole.User, userMessage)
        };
        var response = await _chatClient.GetResponseAsync(messages, cancellationToken: ct);
        return response.Text ?? "{}";
    }
}
