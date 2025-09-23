using CodeLibrary.Models.ValueObjects;
using CSharpFunctionalExtensions;

namespace CodeLibrary.Models;

public class Status
{
    //EF Core
    private Status() { }

    private Status(Name name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public Guid Id { get; private set; }

    public Name Name { get; private set; }

    public static Result<Status> Create(Name name)
    {
        var obj = new Status(name);
        return Result.Success(obj);
    }
}