using OkBoomerAI.ViewModels;

namespace OkBoomerAI.Views;

public partial class BoomerQuizPage : ContentPage
{
    private readonly BoomerQuizViewModel _viewModel;

    public BoomerQuizPage(BoomerQuizViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;

        _viewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(BoomerQuizViewModel.Options))
                WireUpOptionButtons();
            if (e.PropertyName == nameof(BoomerQuizViewModel.HasAnswered) && _viewModel.HasAnswered)
                HighlightAnswers();
        };
    }

    private void WireUpOptionButtons()
    {
        // BindableLayout rebuilds children after ItemsSource changes;
        // defer so the new children exist.
        Dispatcher.Dispatch(() =>
        {
            for (int i = 0; i < OptionsLayout.Children.Count; i++)
            {
                if (OptionsLayout.Children[i] is Button btn)
                {
                    int index = i;
                    btn.Clicked += (s, e) =>
                    {
                        if (_viewModel.SubmitAnswerCommand.CanExecute(index))
                            _viewModel.SubmitAnswerCommand.Execute(index);
                    };
                }
            }
        });
    }

    private void HighlightAnswers()
    {
        int correctIndex = _viewModel.CorrectAnswerIndex;

        for (int i = 0; i < OptionsLayout.Children.Count; i++)
        {
            if (OptionsLayout.Children[i] is Button btn)
            {
                btn.IsEnabled = false;

                if (i == correctIndex)
                    btn.BackgroundColor = Colors.Green;
                else if (i == _viewModel.SelectedIndex)
                    btn.BackgroundColor = Colors.Red;
            }
        }
    }
}
