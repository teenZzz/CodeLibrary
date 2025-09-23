using CodeLibrary.Models.ValueObjects;
using CSharpFunctionalExtensions;

namespace CodeLibrary.Models;

public class Book
{
    private readonly  List<Author> _authors;
    private readonly  List<Tag> _tags;
    
    //EF Core
    private Book() { }
    
    private Book(
        Name name, 
        List<Author> authors, 
        List<Tag> tags, 
        Description? description, 
        Guid statusId)
    {
        Id = Guid.NewGuid();
        Name = name;
        _authors = authors;
        _tags = tags;
        Description = description;
        StatusId = statusId;
    }

    public Guid Id { get; private set; }
    
    public Name Name { get; private set; }
    
    public IReadOnlyList<Author> Authors => _authors;
    
    public IReadOnlyList<Tag> Tags => _tags;

    public Description? Description { get; private set; }

    public Guid StatusId { get; private set; }

    public static Result<Book> Create(
        Name name, 
        List<Author> authors, 
        List<Tag> tags, 
        Description? description, 
        Guid statusId)
    {
        var obj = new Book(name, authors, tags, description, statusId);
        return Result.Success(obj);
    }

}