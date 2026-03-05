using OkBoomerAI.ViewModels;

namespace OkBoomerAI.Views;

public partial class BoomerTranslatorPage : ContentPage
{
    public BoomerTranslatorPage(BoomerTranslatorViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
