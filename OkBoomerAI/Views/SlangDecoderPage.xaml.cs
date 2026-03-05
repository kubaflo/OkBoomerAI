using OkBoomerAI.ViewModels;

namespace OkBoomerAI.Views;

public partial class SlangDecoderPage : ContentPage
{
    public SlangDecoderPage(SlangDecoderViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
