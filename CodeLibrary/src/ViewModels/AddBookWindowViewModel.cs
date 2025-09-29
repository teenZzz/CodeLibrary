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
    
    public AddBookWindowViewModel(IBookQueries bookQueries, CreateBookHandler createBookHandler)
    {
        _bookQueries = bookQueries;
        _createBookHandler = createBookHandler;
        
        AddBookCommand = new RelayCommand(ExecuteAddBook, () => true);
    }
    
    public ICommand AddBookCommand { get; }
    
    public BookDto? CreatedBook { get; private set; }

    public event EventHandler<bool>? CloseRequested;
    
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
            CreatedBook = await _bookQueries.GetByIdAsync(result.Value);
            
            // закроем окно
            CloseRequested?.Invoke(this, true);
        }
        else
        {
            MessageBox.Show(result.Error);
        }

    }
}