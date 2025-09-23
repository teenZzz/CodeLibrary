using CodeLibrary.Models.ValueObjects;
using CSharpFunctionalExtensions;

namespace CodeLibrary.Models;

public class Tag
{
    //EF Core
    private Tag() { }

    private Tag(Name name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public Guid Id { get; private set; }

    public Name Name { get; private set; }

    public static Result<Tag> Create(Name name)
    {
        var obj = new Tag(name);
        return Result.Success(obj);
    }
}