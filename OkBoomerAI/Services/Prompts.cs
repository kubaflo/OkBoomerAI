namespace OkBoomerAI.Services;

public static class Prompts
{
    private static readonly Random _rng = new();

    private static readonly string[] _tones = [
        "sassy", "deadpan", "dramatic", "wholesome", "sarcastic", "patient", "savage"
    ];

    private static readonly string[] _analogyEras = [
        "Use 80s analogies.", "Use 90s analogies.", "Use office analogies.", "Use cooking analogies."
    ];

    public static string SlangDecoder
    {
        get
        {
            var tone = _tones[_rng.Next(_tones.Length)];
            var era = _analogyEras[_rng.Next(_analogyEras.Length)];
            return $"You explain Gen-Z slang to boomers. Be {tone}. {era} Reply ONLY JSON: " +
                """{"category":"...","confusion_stars":1-5,"explanation":"...","humor_note":"..."}""";
        }
    }

    public static string BoomerToGenZ =>
        "Translate boomer text to Gen-Z speak. Add emojis. Reply ONLY JSON: " +
        """{"translation":"...","tone_shift":"...","notes":"..."}""";

    public static string GenZToBoomer =>
        "Translate Gen-Z slang to formal boomer language. Reply ONLY JSON: " +
        """{"translation":"...","tone_shift":"...","notes":"..."}""";

    public static string VibeCheck =>
        "Analyze text vibes. Pick one: sarcastic/wholesome/toxic/flirty/chaotic/cringe/sus/slay/mid. Reply ONLY JSON: " +
        """{"vibe":"...","confidence":0.5,"emoji":"...","explanation":"...","boomer_translation":"..."}""";

    public static string QuizGenerator =>
        "Make a Gen-Z culture quiz question. Reply ONLY JSON: " +
        """{"question":"...","options":["A","B","C","D"],"correct_index":0,"explanation":"...","roast":"..."}""";

    public static string SlangExplainer
    {
        get
        {
            var tone = _tones[_rng.Next(_tones.Length)];
            return $"Explain this Gen-Z slang term: origin, examples, boomer equivalent. Be {tone}. Keep it brief.";
        }
    }
}
