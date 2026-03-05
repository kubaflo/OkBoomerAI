using Microsoft.Extensions.AI;

namespace OkBoomerAI.Services;

public class EmbeddingService : IEmbeddingService
{
    private readonly IEmbeddingGenerator<string, Embedding<float>> _generator;

    public EmbeddingService(IEmbeddingGenerator<string, Embedding<float>> generator)
    {
        _generator = generator;
    }

    public async Task<float[]> GetEmbeddingAsync(string text, CancellationToken ct = default)
    {
        var result = await _generator.GenerateAsync([text], cancellationToken: ct);
        return result[0].Vector.ToArray();
    }

    public async Task<IReadOnlyList<float[]>> GetEmbeddingsAsync(IEnumerable<string> texts, CancellationToken ct = default)
    {
        var result = await _generator.GenerateAsync(texts.ToList(), cancellationToken: ct);
        return result.Select(e => e.Vector.ToArray()).ToList();
    }

    public float CosineSimilarity(float[] a, float[] b)
    {
        if (a.Length != b.Length) return 0f;

        float dot = 0, magA = 0, magB = 0;
        for (int i = 0; i < a.Length; i++)
        {
            dot += a[i] * b[i];
            magA += a[i] * a[i];
            magB += b[i] * b[i];
        }

        var magnitude = MathF.Sqrt(magA) * MathF.Sqrt(magB);
        return magnitude == 0 ? 0 : dot / magnitude;
    }
}
