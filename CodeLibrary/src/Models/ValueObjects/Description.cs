using CSharpFunctionalExtensions;

namespace CodeLibrary.Models.ValueObjects;

public record Description
{
    private Description(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }

    public static Result<Description> Create(string description)
    {
        var obj = new Description(description);
        return Result.Success(obj);
    }
}