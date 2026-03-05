using OkBoomerAI.Models;

namespace OkBoomerAI.Services;

public interface IChatService
{
    Task<string> GetResponseAsync(string systemPrompt, string userMessage, CancellationToken ct = default);
    IAsyncEnumerable<string> GetStreamingResponseAsync(string systemPrompt, List<ChatMessage> history, CancellationToken ct = default);
    Task<string> GetStructuredResponseAsync(string systemPrompt, string userMessage, string jsonSchema, CancellationToken ct = default);
}
