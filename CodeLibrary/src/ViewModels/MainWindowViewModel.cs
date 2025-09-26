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

        _ = LoadAsync();
    }
    
    public ObservableCollection<BookDto> Books { get; } = [];

    public ICommand SearchCommand { get; }

    public ICommand AddBookCommand { get; }

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
}