using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OkBoomerAI.Models;
using OkBoomerAI.Services;

namespace OkBoomerAI.ViewModels;

public partial class SlangDictionaryViewModel : ObservableObject
{
    private readonly SlangDataService _slangData;
    private readonly IEmbeddingService _embeddingService;
    private readonly IChatService _chatService;

    private List<SlangEntry> _allEntries = [];
    private Dictionary<string, float[]> _embeddings = [];

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private bool _isLoadingEmbeddings;

    [ObservableProperty]
    private string _selectedExplanation = string.Empty;

    [ObservableProperty]
    private SlangEntry? _selectedEntry;

    [ObservableProperty]
    private bool _showExplanation;

    public ObservableCollection<SlangEntry> FilteredEntries { get; } = [];

    public SlangDictionaryViewModel(
        SlangDataService slangData,
        IEmbeddingService embeddingService,
        IChatService chatService)
    {
        _slangData = slangData;
        _embeddingService = embeddingService;
        _chatService = chatService;
    }

    [RelayCommand]
    private async Task LoadData()
    {
        if (_allEntries.Count > 0) return;

        IsBusy = true;
        try
        {
            _allEntries = await _slangData.GetEntriesAsync();
            FilteredEntries.Clear();
            foreach (var entry in _allEntries)
                FilteredEntries.Add(entry);

            _ = Task.Run(PrecomputeEmbeddingsAsync);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task PrecomputeEmbeddingsAsync()
    {
        IsLoadingEmbeddings = true;
        try
        {
            foreach (var entry in _allEntries)
            {
                var text = $"{entry.Term}: {entry.Definition}";
                var embedding = await _embeddingService.GetEmbeddingAsync(text);
                _embeddings[entry.Term] = embedding;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Embedding precompute failed: {ex.Message}");
        }
        finally
        {
            IsLoadingEmbeddings = false;
        }
    }

    [RelayCommand]
    private async Task Search()
    {
        var query = SearchText?.Trim();
        if (string.IsNullOrEmpty(query))
        {
            FilteredEntries.Clear();
            foreach (var entry in _allEntries)
                FilteredEntries.Add(entry);
            return;
        }

        IsBusy = true;
        try
        {
            if (_embeddings.Count > 0)
            {
                var queryEmbedding = await _embeddingService.GetEmbeddingAsync(query);

                var ranked = _allEntries
                    .Where(e => _embeddings.ContainsKey(e.Term))
                    .Select(e => new { Entry = e, Score = _embeddingService.CosineSimilarity(queryEmbedding, _embeddings[e.Term]) })
                    .OrderByDescending(x => x.Score)
                    .Take(15)
                    .ToList();

                FilteredEntries.Clear();
                foreach (var item in ranked)
                    FilteredEntries.Add(item.Entry);
            }
            else
            {
                var results = await _slangData.SearchAsync(query);
                FilteredEntries.Clear();
                foreach (var entry in results)
                    FilteredEntries.Add(entry);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ExplainTerm(SlangEntry? entry)
    {
        if (entry == null) return;

        SelectedEntry = entry;
        ShowExplanation = true;
        SelectedExplanation = "Generating explanation... 🧠";

        try
        {
            var explanation = await _chatService.GetResponseAsync(
                Prompts.SlangExplainer,
                $"Explain the Gen-Z slang term: \"{entry.Term}\" (meaning: {entry.Definition})");

            SelectedExplanation = explanation;
        }
        catch (Exception ex)
        {
            SelectedExplanation = $"Couldn't explain this one 💀: {ex.Message}";
        }
    }

    [RelayCommand]
    private void CloseExplanation()
    {
        ShowExplanation = false;
        SelectedExplanation = string.Empty;
    }

    partial void OnSearchTextChanged(string value)
    {
        if (string.IsNullOrEmpty(value) && _allEntries.Count > 0)
        {
            FilteredEntries.Clear();
            foreach (var entry in _allEntries)
                FilteredEntries.Add(entry);
        }
    }
}
