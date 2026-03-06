using System.Runtime.CompilerServices;

namespace OkBoomerAI.Services;

/// <summary>
/// Mock chat service for simulator testing where Apple Intelligence is unavailable.
/// Returns canned humorous responses so all UI features can be verified.
/// </summary>
public class MockChatService : IChatService
{
    private static readonly Random _rng = new();

    private static readonly string[] DecoderResponses =
    [
        "OK so \"no cap\" basically means \"I'm not lying\" — think of it like \"scout's honor\" but for people who've never been to Boy Scouts 💀\n\nIt comes from the idea of \"capping\" = lying. So \"no cap\" = no lie.\n\n**Boomer Translation:** \"I swear on my mother's grave\"",
        "\"Fr fr\" means \"for real for real\" — it's when one \"for real\" isn't enough to convey sincerity. Like saying \"I'm serious\" but twice because nobody believes you the first time 😭\n\n**Boomer Translation:** \"I mean it, honestly and truly\"",
        "\"Slay\" means you absolutely crushed it. Demolished. Left no survivors. It's the highest compliment Gen-Z can give.\n\nThink of it like saying \"you knocked it out of the park\" but with ✨ more sparkle ✨\n\n**Boomer Translation:** \"That was exceptionally well done, Margaret.\"",
        "\"Bussin\" means something is REALLY good, usually food. If grandma's Thanksgiving turkey is bussin, that's the ultimate seal of approval 🍗\n\nYou're basically saying \"this slaps\" which also means... it's good. Yeah we have 47 words for 'good'.\n\n**Boomer Translation:** \"This is quite delicious\"",
        "\"Rizz\" is short for charisma. If someone has rizz, they can charm anyone. Think of it as being smooth — like a young George Clooney but on TikTok.\n\nYou can also \"rizz someone up\" which means to flirt successfully.\n\n**Boomer Translation:** \"He's quite the charmer\" 🎩",
    ];

    private static readonly string[] VibeJsonResponses =
    [
        """{"vibe":"sarcastic","confidence":0.92,"emoji":"💅","explanation":"This text is dripping with sarcasm. Every word is carefully chosen to sound nice while meaning the exact opposite. Chef's kiss of passive destruction.","boomer_translation":"They're being very sarcastic, dear."}""",
        """{"vibe":"wholesome","confidence":0.88,"emoji":"🥰","explanation":"This is genuinely sweet and heartwarming. No hidden agenda, no passive aggression — just pure vibes. Rare on the internet tbh.","boomer_translation":"This is a very nice and sincere message."}""",
        """{"vibe":"chaotic","confidence":0.85,"emoji":"🤪","explanation":"This has unhinged energy. The author clearly chose violence today and we're all just along for the ride. Zero thoughts, full send.","boomer_translation":"This person seems quite eccentric."}""",
        """{"vibe":"sus","confidence":0.79,"emoji":"👀","explanation":"Something about this doesn't add up. The vibe is off. Trust issues activated. This text has 'read the fine print' energy.","boomer_translation":"This seems rather suspicious."}""",
    ];

    private static readonly string[] TranslationJsonResponses =
    [
        """{"translation":"bestie that's giving main character energy fr fr no cap 💅✨","tone_shift":"Went from corporate boardroom to TikTok comment section","notes":"Replaced formal language with Gen-Z affirmations and added obligatory emoji garnish"}""",
        """{"translation":"I would like to formally express my sincere agreement with the aforementioned proposition.","tone_shift":"Went from chaotic internet to congressional hearing","notes":"Removed all slang, emojis, and joy. Added formality and a slight air of superiority."}""",
    ];

    private static readonly string[] QuizJsonResponses =
    [
        """{"question":"What does it mean when someone says 'that's lowkey fire'?","options":["The temperature is slightly warm","It's secretly really good","There's a small fire nearby","They're whispering about arson"],"correct_index":1,"explanation":"'Lowkey' means somewhat or secretly, and 'fire' means excellent. So 'lowkey fire' = secretly really good.","roast":"Bestie you thought there was an actual fire? 💀 Touch grass immediately."}""",
        """{"question":"If someone says you have 'negative aura', what do they mean?","options":["You need a shower","You did something embarrassing","You have bad posture","Your horoscope is off"],"correct_index":1,"explanation":"'Aura' in Gen-Z means your social reputation or presence. Negative aura = you just did something embarrassing that lowered your social standing. Often measured in points like '-1000 aura'.","roast":"Tell me you're a boomer without telling me you're a boomer 😭"}""",
        """{"question":"What is 'brainrot'?","options":["A medical condition","When too much internet content deteriorates your thinking","A type of dance move","A brand of energy drink"],"correct_index":1,"explanation":"Brainrot refers to when excessive consumption of internet content (especially TikTok) starts affecting how you think and speak. If you know what 'skibidi toilet' is, you might have it.","roast":"The fact that you don't know this means you DON'T have brainrot. That's actually a W for you, boomer. 🏆"}""",
    ];

    public async Task<string> GetResponseAsync(string systemPrompt, string userMessage, CancellationToken ct = default)
    {
        await Task.Delay(800, ct); // simulate thinking
        return DecoderResponses[_rng.Next(DecoderResponses.Length)];
    }

    public async IAsyncEnumerable<string> GetStreamingResponseAsync(
        string systemPrompt,
        List<Models.ChatMessage> history,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        var response = DecoderResponses[_rng.Next(DecoderResponses.Length)];

        // Simulate streaming word by word
        foreach (var word in response.Split(' '))
        {
            await Task.Delay(50, ct);
            yield return word + " ";
        }
    }

    public async Task<string> GetStructuredResponseAsync(
        string systemPrompt,
        string userMessage,
        string jsonSchema,
        CancellationToken ct = default)
    {
        await Task.Delay(1000, ct);

        if (systemPrompt.Contains("vibe", StringComparison.OrdinalIgnoreCase))
            return VibeJsonResponses[_rng.Next(VibeJsonResponses.Length)];

        if (systemPrompt.Contains("quiz", StringComparison.OrdinalIgnoreCase))
            return QuizJsonResponses[_rng.Next(QuizJsonResponses.Length)];

        if (systemPrompt.Contains("translat", StringComparison.OrdinalIgnoreCase))
            return TranslationJsonResponses[_rng.Next(TranslationJsonResponses.Length)];

        return """{"error":"Unknown prompt type"}""";
    }
}
