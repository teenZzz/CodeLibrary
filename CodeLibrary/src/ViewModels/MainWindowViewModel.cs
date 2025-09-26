using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CodeLibrary.Common;
using CodeLibrary.Models.DTOs;
using CodeLibrary.Models.Requests;
using CodeLibrary.UseCases.Handlers;

namespace CodeLibrary.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly CreateBookHandler _createBookHandler;
    public MainWindowViewModel(CreateBookHandler createBookHandler)
    {
        _createBookHandler = createBookHandler;

        //Demo
        Books.Add(new BookDto
        {
            Title = "Clr Via C#",
            Author = "Д. Рихтер",
            Tag = ".NET",
            Description = "C#",
            Status = "Читаю"
        });
        Books.Add(new BookDto
        {
            Title = "Clr Via C#",
            Author = "Д. Рихтер",
            Tag = ".NET",
            Description = "C#",
            Status = "Читаю"
        });

        SearchCommand = new RelayCommand(ExecuteSearch, () => true);
        AddBookCommand = new RelayCommand(ExecuteAddBook, () => true);
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
        var request = new CreateBookRequest("CLR VIA C#", "Рихтер", "Джеффри", "", "C#", "Учебная книга", "В планах");
        
        await _createBookHandler.Handle(request);
    }
}