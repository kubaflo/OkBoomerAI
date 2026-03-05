using OkBoomerAI.ViewModels;

namespace OkBoomerAI.Views;

public partial class BoomerQuizPage : ContentPage
{
    private readonly BoomerQuizViewModel _viewModel;

    public BoomerQuizPage(BoomerQuizViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Wire up option button clicks
        var optionsLayout = this.FindByName<VerticalStackLayout>("OptionsLayout");
        // Button click handling is done via command binding in XAML
    }
}
