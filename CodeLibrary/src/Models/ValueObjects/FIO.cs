using CSharpFunctionalExtensions;

namespace CodeLibrary.Models.ValueObjects;

public record FIO
{
    private FIO(string surname, string firstName, string? patronymic)
    {
        Surname = surname;
        FirstName = firstName;
        Patronymic = patronymic;
    }

    public string Surname { get; private set; }

    public string FirstName { get; private set; }

    public string? Patronymic { get; private set; }

    public static Result<FIO> Create(string surname, string firstName, string? patronymic)
    {
        var obj = new FIO(surname, firstName, patronymic);
        return Result.Success(obj);
    }
}