using System.Windows.Media.Animation;
using CSharpFunctionalExtensions;

namespace CodeLibrary.Models;

public class BookTag
{
    //EF Core
    private BookTag() { }

    private BookTag(Guid book, Guid tag)
    {
        Book = book;
        Tag = tag;
    }

    public Guid Book { get; private set; }

    public Guid Tag { get; private set; }

    public static Result<BookTag> Create(Guid book, Guid tag)
    {
        var obj = new BookTag(book, tag);
        return Result.Success(obj);
    }
}