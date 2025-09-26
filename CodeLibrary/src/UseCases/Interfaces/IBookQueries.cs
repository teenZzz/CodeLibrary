using CodeLibrary.Models.DTOs;

namespace CodeLibrary.UseCases.InterfacesRepositories;

public interface IBookQueries
{
    Task<List<BookDto>> GetAllAsync();
    Task<BookDto?> GetByIdAsync(Guid id);
}