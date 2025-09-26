namespace CodeLibrary.Models.DTOs;

public record BookDto
{
    public Guid Id { get; init; }
    public required string Title { get; init; }

    public required string Author { get; init; }

    public required string Tag { get; init; }

    public string? Description { get; init; }

    public required string Status { get; init; }
}