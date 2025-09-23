using CodeLibrary.Common;
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
        if (string.IsNullOrWhiteSpace(surname))
            return Result.Failure<FIO>("Surname cannot be empty!");
        
        if (string.IsNullOrWhiteSpace(firstName))
            return Result.Failure<FIO>("Firstname cannot be empty!");

        if (surname.Length > Consts.Text.MAX_LENGTH)
            return Result.Failure<FIO>("Incorrect surname length!");
        
        if (firstName.Length > Consts.Text.MAX_LENGTH)
            return Result.Failure<FIO>("Incorrect firstname length!");
        
        if (patronymic != null && patronymic.Length > Consts.Text.MAX_LENGTH)
            return Result.Failure<FIO>("Incorrect patronymic length!");
        
        var obj = new FIO(surname, firstName, patronymic);
        return Result.Success(obj);
    }
}