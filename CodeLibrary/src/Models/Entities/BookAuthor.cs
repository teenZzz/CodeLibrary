using CSharpFunctionalExtensions;

namespace CodeLibrary.Models;

public class BookAuthor
{
    //EF Core
    private BookAuthor() { }

    private BookAuthor(Guid bookId, Guid authorId)
    {
        BookId = bookId;
        AuthorId = authorId;
    }

    public Guid BookId { get; private set; }

    public Guid AuthorId { get; private set; }

    public static Result<BookAuthor> Create(Guid bookId, Guid authorId)
    {
        var obj = new BookAuthor(bookId, authorId);
        return Result.Success(obj);
    }
}