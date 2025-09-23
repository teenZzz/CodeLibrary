using CodeLibrary.Models.ValueObjects;
using CSharpFunctionalExtensions;

namespace CodeLibrary.Models;

public class Author
{
    //EF Core
    private Author() { }

    private Author(FIO fio)
    {
        Id = Guid.NewGuid();
        Fio = fio;
    }

    public Guid Id { get; private set; }

    public FIO Fio { get; private set; }

    public static Result<Author> Create(FIO fio)
    {
        var obj = new Author(fio);
        return Result.Success(obj);
    }
    
}