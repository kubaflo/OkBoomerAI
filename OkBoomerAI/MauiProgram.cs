using CommunityToolkit.Maui;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using OkBoomerAI.Services;
using OkBoomerAI.ViewModels;
using OkBoomerAI.Views;

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
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// AI Services — Apple Intelligence (requires iOS/macOS 26+)
#if IOS || MACCATALYST
#pragma warning disable CA1416
		builder.Services.AddSingleton<IChatClient>(new AppleIntelligenceChatClient());
		builder.Services.AddSingleton<IEmbeddingGenerator<string, Embedding<float>>>(new NLEmbeddingGenerator());
#pragma warning restore CA1416
#endif

		builder.Services.AddSingleton<IChatService, AppleIntelligenceChatService>();
		builder.Services.AddSingleton<IEmbeddingService, EmbeddingService>();
		builder.Services.AddSingleton<SlangDataService>();

		// ViewModels
		builder.Services.AddTransient<SlangDecoderViewModel>();
		builder.Services.AddTransient<BoomerTranslatorViewModel>();
		builder.Services.AddTransient<VibeCheckViewModel>();
		builder.Services.AddTransient<SlangDictionaryViewModel>();
		builder.Services.AddTransient<BoomerQuizViewModel>();

		// Pages
		builder.Services.AddTransient<SlangDecoderPage>();
		builder.Services.AddTransient<BoomerTranslatorPage>();
		builder.Services.AddTransient<VibeCheckPage>();
		builder.Services.AddTransient<SlangDictionaryPage>();
		builder.Services.AddTransient<BoomerQuizPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
