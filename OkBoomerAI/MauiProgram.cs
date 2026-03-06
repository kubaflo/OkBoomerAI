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

		// AI Services — use Apple Intelligence on real devices, mock on simulator
		bool useRealAI = false;
#if IOS || MACCATALYST
#pragma warning disable CA1416
		if (OperatingSystem.IsIOSVersionAtLeast(26) || OperatingSystem.IsMacCatalystVersionAtLeast(26))
		{
			try
			{
				builder.Services.AddSingleton<IChatClient>(new AppleIntelligenceChatClient());
				builder.Services.AddSingleton<IChatService, AppleIntelligenceChatService>();
				useRealAI = true;
			}
			catch
			{
				// Apple Intelligence unavailable (simulator or unsupported device)
			}
		}

		// Embeddings work on older iOS via NaturalLanguage framework
		try
		{
			builder.Services.AddSingleton<IEmbeddingGenerator<string, Embedding<float>>>(new NLEmbeddingGenerator());
			builder.Services.AddSingleton<IEmbeddingService, EmbeddingService>();
		}
		catch
		{
			// NLEmbedding unavailable — dictionary will use text search fallback
		}
#pragma warning restore CA1416
#endif

		if (!useRealAI)
		{
			System.Diagnostics.Debug.WriteLine("⚠️ Apple Intelligence unavailable — using MockChatService");
			builder.Services.AddSingleton<IChatService, MockChatService>();
		}

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
