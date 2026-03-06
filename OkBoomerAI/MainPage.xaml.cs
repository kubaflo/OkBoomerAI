using Microsoft.AspNetCore.Components.WebView;
using Microsoft.AspNetCore.Components.WebView.Maui;

namespace OkBoomerAI;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();

#if IOS || MACCATALYST
		blazorWebView.BlazorWebViewInitialized += OnBlazorWebViewInitializediOS;
#endif
	}

#if IOS || MACCATALYST
	private void OnBlazorWebViewInitializediOS(object? sender, BlazorWebViewInitializedEventArgs e)
	{
		var wkWebView = e.WebView;
		wkWebView.Opaque = false;
		wkWebView.BackgroundColor = UIKit.UIColor.FromRGB(0x0B, 0x10, 0x26);
		wkWebView.ScrollView.BackgroundColor = UIKit.UIColor.FromRGB(0x0B, 0x10, 0x26);
		wkWebView.ScrollView.ContentInsetAdjustmentBehavior = UIKit.UIScrollViewContentInsetAdjustmentBehavior.Never;
		wkWebView.ScrollView.ContentInset = UIKit.UIEdgeInsets.Zero;
		wkWebView.ScrollView.ScrollIndicatorInsets = UIKit.UIEdgeInsets.Zero;
	}
#endif
}
