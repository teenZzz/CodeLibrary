using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CodeLibrary.Common;
using CodeLibrary.Models.DTOs;
using CodeLibrary.Models.Requests;
using CodeLibrary.UseCases.Handlers;
using CodeLibrary.UseCases.InterfacesRepositories;

namespace CodeLibrary.ViewModels;

public class AddBookWindowViewModel : ViewModelBase
{
    private readonly IBookQueries _bookQueries;
    private readonly CreateBookHandler _createBookHandler;
    private readonly ILoadQueries _loadQueries;
    
    private string _title = string.Empty;
    private string _authorSurname = string.Empty;
    private string _authorFirstName = string.Empty;
    private string _authorPatronymic = string.Empty;
    private string _selectedTag = string.Empty;
    private string _description = string.Empty;
    private string _selectedStatus = string.Empty;

    private readonly ObservableCollection<string> _tags = new();
    private readonly ObservableCollection<string> _statuses = new();
    
    public AddBookWindowViewModel(IBookQueries bookQueries, ILoadQueries loadQueries, CreateBookHandler createBookHandler)
    {
        _bookQueries = bookQueries;
        _loadQueries = loadQueries;
        _createBookHandler = createBookHandler;
        
        AddBookCommand = new RelayCommand(ExecuteAddBook, () => true);
        _ = LoadAsync();
    }
    
    public ICommand AddBookCommand { get; }
    
    public BookDto? CreatedBook { get; private set; }

    public event EventHandler<bool>? CloseRequested;

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public string AuthorSurname
    {
        get => _authorSurname;
        set => SetProperty(ref _authorSurname, value);
    }

    public string AuthorFirstName
    {
        get => _authorFirstName;
        set => SetProperty(ref _authorFirstName, value);
    }

    public string AuthorPatronymic
    {
        get => _authorPatronymic;
        set => SetProperty(ref _authorPatronymic, value);
    }

    public string Tag
    {
        get => _selectedTag;
        set => SetProperty(ref _selectedTag, value);
    }
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public string Status
    {
        get => _selectedStatus;
        set => SetProperty(ref _selectedStatus, value);
    }

    public ObservableCollection<string> Tags => _tags;
    
    public ObservableCollection<string> Statuses => _statuses;

    private async Task ExecuteAddBook()
    {
        var request = new CreateBookRequest(Title,
            AuthorSurname,
            AuthorFirstName,
            AuthorPatronymic,
            Tag,
            Description,
            Status);

        var result = await _createBookHandler.Handle(request);

        if (result.IsSuccess)
        {
            // подтягиваем только что созданную книгу и добавляем в список
            CreatedBook = await _bookQueries.GetByIdAsync(result.Value);
            
            // закроем окно
            CloseRequested?.Invoke(this, true);
        }
        else
        {
            MessageBox.Show(result.Error);
        }

    }

    private async Task LoadAsync()
    {
        try
        {
            var statuses = await _loadQueries.GetAllStatusesAsync();
            _statuses.Clear();
            foreach (var s in statuses)
                _statuses.Add(s);

            var tags = await _loadQueries.GetAllTagsAsync();
            _tags.Clear();
            foreach (var t in tags)
                _tags.Add(t);

            // при желании можно выбрать дефолты:
            if (string.IsNullOrWhiteSpace(Status) && _statuses.Count > 0) Status = _statuses[0];
            if (string.IsNullOrWhiteSpace(Tag) && _tags.Count > 0) Tag = _tags[0];
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Не удалось загрузить справочники: {ex.Message}");
        }
    }
}