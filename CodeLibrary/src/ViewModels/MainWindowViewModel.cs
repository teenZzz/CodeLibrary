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
    public MainWindowViewModel(IServiceProvider services, IBookQueries bookQueries, CreateBookHandler createBookHandler, DeleteBookHandler deleteBookHandler)
    {
        _services = services;
        
        _bookQueries = bookQueries;
        _createBookHandler = createBookHandler;
        _deleteBookHandler = deleteBookHandler;

        SearchCommand = new RelayCommand(ExecuteSearch, () => true);
        AddBookCommand = new RelayCommand(ExecuteAddBook, () => true);
        
        DeleteBookCommand = new RelayCommand<BookDto?>(ExecuteDeleteBook, b => b != null);

        _ = LoadAsync();
    }
    
    public ObservableCollection<BookDto> Books { get; } = [];

    public ICommand SearchCommand { get; }

    public ICommand AddBookCommand { get; }
    
    public ICommand DeleteBookCommand { get; }

    private async Task ExecuteSearch()
    {
        MessageBox.Show("Search");
    }

    private async Task ExecuteAddBook()
    {
        /*var request = new CreateBookRequest("CLR VIA C#",
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
        */

        var window = _services.GetRequiredService<AddBookWindow>();
        window.Owner = Application.Current?.MainWindow;
        
        if (window.ShowDialog() == true)
        {
            var vm = (AddBookWindowViewModel)window.DataContext;
            if (vm.CreatedBook != null)
                Books.Add(vm.CreatedBook);
        }

    }
    
    private async Task LoadAsync()
    {
        var items = await _bookQueries.GetAllAsync();
        Books.Clear();
        foreach (var dto in items)
            Books.Add(dto);
    }

    private async Task ExecuteDeleteBook(BookDto? book)
    {
        if (book is null) return;

        var result = await _deleteBookHandler.Handle(book.Id);
        if (result.IsSuccess)
            Books.Remove(book);
        else
            MessageBox.Show(result.Error);
    }
}