using CodeLibrary.Common;
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
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Description>("Description cannot be empty!");

        if (description.Length < Consts.Text.MIN_LENGTH || description.Length > Consts.Text.MAX_LENGTH)
            return Result.Failure<Description>("Incorrect description length!");
        
        var obj = new Description(description);
        return Result.Success(obj);
    }
}