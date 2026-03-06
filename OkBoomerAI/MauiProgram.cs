using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using OkBoomerAI.Services;

#if IOS || MACCATALYST
using Microsoft.Maui.Essentials.AI;
using NaturalLanguage;
#endif

namespace OkBoomerAI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        // Apple Intelligence AI Services
#if IOS || MACCATALYST
#pragma warning disable CA1416
        builder.Services.AddSingleton<IChatClient>(new AppleIntelligenceChatClient());
        builder.Services.AddSingleton<IEmbeddingGenerator<string, Embedding<float>>>(new NLEmbeddingGenerator());
#pragma warning restore CA1416
#endif

        builder.Services.AddSingleton<IChatService, AppleIntelligenceChatService>();
        builder.Services.AddSingleton<IEmbeddingService, EmbeddingService>();
        builder.Services.AddSingleton<SlangDataService>();
        builder.Services.AddSingleton<HistoryService>();

        return builder.Build();
    }
}
