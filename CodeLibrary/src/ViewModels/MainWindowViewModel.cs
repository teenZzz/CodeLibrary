using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CodeLibrary.Common;
using CodeLibrary.Models.DTOs;
using CodeLibrary.Models.Requests;
using CodeLibrary.UseCases.Handlers;
using CodeLibrary.UseCases.InterfacesRepositories;
using CodeLibrary.Views;
using Microsoft.Extensions.DependencyInjection;

namespace CodeLibrary.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly IServiceProvider _services;
    private readonly IBookQueries _bookQueries;
    private readonly CreateBookHandler _createBookHandler;
    private readonly DeleteBookHandler _deleteBookHandler;
    
    private List<BookDto> _allBooks = new();
    private string _searchText = string.Empty;
    public MainWindowViewModel(IServiceProvider services, IBookQueries bookQueries, CreateBookHandler createBookHandler, DeleteBookHandler deleteBookHandler)
    {
        _services = services;
        
        _bookQueries = bookQueries;
        _createBookHandler = createBookHandler;
        _deleteBookHandler = deleteBookHandler;

        SearchCommand = new RelayCommand(async () => await ExecuteSearch(), () => true);
        AddBookCommand = new RelayCommand(async () => await ExecuteAddBook(), () => true);
        DeleteBookCommand = new RelayCommand<BookDto?>(ExecuteDeleteBook, b => b != null);

        _ = LoadAsync();
    }
    
    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }
    
    public ObservableCollection<BookDto> Books { get; } = [];

    public ICommand SearchCommand { get; }

    public ICommand AddBookCommand { get; }
    
    public ICommand DeleteBookCommand { get; }

    private async Task ExecuteSearch()
    {
        var q = (SearchText ?? string.Empty).Trim();

        // Пустой запрос — показать всё
        if (string.IsNullOrEmpty(q))
        {
            Books.Clear();
            foreach (var b in _allBooks)
                Books.Add(b);
            return;
        }

        // Фильтрация по Title (без учёта регистра). Хочешь точное совпадение — замени на Equals.
        var matches = _allBooks.Where(b =>
            (b.Title ?? string.Empty).IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0);

        Books.Clear();
        foreach (var b in matches)
            Books.Add(b);
    }

    private async Task ExecuteAddBook()
    {
        var window = _services.GetRequiredService<AddBookWindow>();
        window.Owner = Application.Current?.MainWindow;

        if (window.ShowDialog() == true)
        {
            var vm = (AddBookWindowViewModel)window.DataContext;
            if (vm.CreatedBook != null)
            {
                // Обновляем и кэш, и отображаемую коллекцию
                _allBooks.Add(vm.CreatedBook);
                Books.Add(vm.CreatedBook);
            }
        }
    }
    
    private async Task LoadAsync()
    {
        var items = await _bookQueries.GetAllAsync();

        _allBooks = items.ToList(); // <-- важная строка: заполняем кэш

        Books.Clear();
        foreach (var dto in _allBooks)
            Books.Add(dto);
    }

    private async Task ExecuteDeleteBook(BookDto? book)
    {
        if (book is null) return;

        var result = await _deleteBookHandler.Handle(book.Id);
        if (result.IsSuccess)
        {
            _allBooks.Remove(book); // синхронизируем кэш
            Books.Remove(book);     // и витрину
        }
        else
        {
            MessageBox.Show(result.Error);
        }
    }
}