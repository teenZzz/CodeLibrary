using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CodeLibrary.Common;
using CodeLibrary.Models.DTOs;
using CodeLibrary.Models.Requests;
using CodeLibrary.UseCases.Handlers;
using CodeLibrary.UseCases.InterfacesRepositories;

namespace CodeLibrary.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly IBookQueries _bookQueries;
    private readonly CreateBookHandler _createBookHandler;
    public MainWindowViewModel(IBookQueries bookQueries, CreateBookHandler createBookHandler)
    {
        _bookQueries = bookQueries;
        _createBookHandler = createBookHandler;

        SearchCommand = new RelayCommand(ExecuteSearch, () => true);
        AddBookCommand = new RelayCommand(ExecuteAddBook, () => true);
        
        EditBookCommand = new RelayCommand<BookDto?>(ExecuteEditBook, b => b != null);
        DeleteBookCommand = new RelayCommand<BookDto?>(ExecuteDeleteBook, b => b != null);

        _ = LoadAsync();
    }
    
    public ObservableCollection<BookDto> Books { get; } = [];

    public ICommand SearchCommand { get; }

    public ICommand AddBookCommand { get; }
    
    public ICommand EditBookCommand { get; }
    
    public ICommand DeleteBookCommand { get; }

    private async Task ExecuteSearch()
    {
        MessageBox.Show("Search");
    }

    private async Task ExecuteAddBook()
    {
        var request = new CreateBookRequest("CLR VIA C#",
            "Рихтер",
            "Джеффри",
            "",
            "C#",
            "Учебная книга",
            "В планах");
        
        var result = await _createBookHandler.Handle(request);
        
        if (result.IsSuccess)
        {
            // подтягиваем только что созданную книгу и добавляем в список
            var dto = await _bookQueries.GetByIdAsync(result.Value);
            if (dto is not null)
                Books.Add(dto);
        }
        else
        {
            MessageBox.Show(result.Error);
        }
        
    }
    
    private async Task LoadAsync()
    {
        var items = await _bookQueries.GetAllAsync();
        Books.Clear();
        foreach (var dto in items)
            Books.Add(dto);
    }
    
    
    private Task ExecuteEditBook(BookDto? book)
    {
        if (book is null) return Task.CompletedTask;
        MessageBox.Show($"Edit: {book.Title}");
        return Task.CompletedTask;
    }

    private Task ExecuteDeleteBook(BookDto? book)
    {
        if (book is null) return Task.CompletedTask;
        Books.Remove(book);
        MessageBox.Show($"Deleted: {book.Title}");
        return Task.CompletedTask;
    }
}