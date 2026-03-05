using OkBoomerAI.ViewModels;

namespace OkBoomerAI.Views;

public partial class VibeCheckPage : ContentPage
{
    public VibeCheckPage(VibeCheckViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
