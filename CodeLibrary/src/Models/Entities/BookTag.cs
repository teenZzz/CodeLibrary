using System.Windows.Media.Animation;
using CSharpFunctionalExtensions;

namespace CodeLibrary.Models;

public class BookTag
{
    //EF Core
    private BookTag() { }

    private BookTag(Guid book, Guid tag)
    {
        BookId = book;
        TagId = tag;
    }

    public Guid BookId { get; private set; }

    public Guid TagId { get; private set; }

    public static Result<BookTag> Create(Guid book, Guid tag)
    {
        var obj = new BookTag(book, tag);
        return Result.Success(obj);
    }
}