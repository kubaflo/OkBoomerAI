using OkBoomerAI.ViewModels;

namespace OkBoomerAI.Views;

public partial class SlangDictionaryPage : ContentPage
{
    private readonly SlangDictionaryViewModel _viewModel;

    public SlangDictionaryPage(SlangDictionaryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadDataCommand.ExecuteAsync(null);
    }
}
