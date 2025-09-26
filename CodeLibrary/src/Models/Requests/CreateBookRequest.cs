namespace CodeLibrary.Models.Requests;

public record CreateBookRequest(string Name, string AuthorSurname, string AuthorFirstName, string? AuthorPatronymic, string Tag, string? Description, string  Status);