using CodeLibrary.Models;
using CodeLibrary.Models.DTOs;
using CSharpFunctionalExtensions;

namespace CodeLibrary.UseCases.InterfacesRepositories;

public interface IBookRepository
{
    Task<Result<Guid>> Add(Book book);
    Task<Result> DeleteAsync(Guid id);
    Task<Book?> GetByIdAsync(Guid id);
}