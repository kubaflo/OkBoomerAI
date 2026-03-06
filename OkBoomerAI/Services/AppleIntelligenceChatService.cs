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
            new(ChatRole.System, systemPrompt + "\n\nRespond with ONLY valid JSON matching this schema (no markdown, no backticks, no extra text):\n" + jsonSchema),
            new(ChatRole.User, userMessage)
        };
        var response = await _chatClient.GetResponseAsync(messages, cancellationToken: ct);
        return ExtractJson(response.Text ?? "{}");
    }

    private static string ExtractJson(string text)
    {
        text = text.Trim();
        // Strip markdown code fences
        if (text.StartsWith("```"))
        {
            var firstNewline = text.IndexOf('\n');
            if (firstNewline > 0) text = text[(firstNewline + 1)..];
            if (text.EndsWith("```")) text = text[..^3];
            text = text.Trim();
        }
        // Find first { and last }
        var start = text.IndexOf('{');
        var end = text.LastIndexOf('}');
        if (start >= 0 && end > start)
            return text[start..(end + 1)];
        return text;
    }
}
