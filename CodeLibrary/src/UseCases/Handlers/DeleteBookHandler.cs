using CodeLibrary.UseCases.InterfacesRepositories;
using CSharpFunctionalExtensions;

namespace CodeLibrary.UseCases.Handlers;

public class DeleteBookHandler
{
    private readonly IBookRepository _books;
    
    public DeleteBookHandler(IBookRepository books)
    {
        _books = books;
    }

    public Task<Result> Handle(Guid id)
    {
        return _books.DeleteAsync(id);
    }
}