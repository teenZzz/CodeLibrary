using CSharpFunctionalExtensions;

namespace CodeLibrary.Models.ValueObjects;

public record Name
{
    private Name(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }

    public static Result<Name> Create(string name)
    {
        var obj = new Name(name);
        return Result.Success(obj);
    }
}