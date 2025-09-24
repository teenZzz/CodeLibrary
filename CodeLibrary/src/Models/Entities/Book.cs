using CodeLibrary.Models.ValueObjects;
using CSharpFunctionalExtensions;

namespace CodeLibrary.Models;

public class Book
{
    private readonly  List<BookAuthor> _bookAuthors;
    private readonly  List<BookTag> _bookTags;
    
    //EF Core
    private Book() { }
    
    private Book(
        Name name, 
        List<BookAuthor> bookAuthors, 
        List<BookTag> bookTags, 
        Description? description, 
        Guid statusId)
    {
        Id = Guid.NewGuid();
        Name = name;
        _bookAuthors = bookAuthors;
        _bookTags = bookTags;
        Description = description;
        StatusId = statusId;
    }

    public Guid Id { get; private set; }
    
    public Name Name { get; private set; }
    
    public IReadOnlyList<BookAuthor> BookAuthors => _bookAuthors;
    
    public IReadOnlyList<BookTag> BookTags => _bookTags;

    public Description? Description { get; private set; }

    public Guid StatusId { get; private set; }

    public static Result<Book> Create(
        Name name, 
        List<BookAuthor> bookAuthors, 
        List<BookTag> bookTags, 
        Description? description, 
        Guid statusId)
    {
        var obj = new Book(name, bookAuthors, bookTags, description, statusId);
        return Result.Success(obj);
    }

}