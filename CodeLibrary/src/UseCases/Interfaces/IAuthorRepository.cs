using CodeLibrary.Models;
using CodeLibrary.Models.ValueObjects;
using CSharpFunctionalExtensions;

namespace CodeLibrary.UseCases.InterfacesRepositories;

public interface IAuthorRepository
{
    Task<Result<Guid>> Add(Author author);
    
    Task<Author?> FindByFioAsync(FIO fio);

    Task<Result<Guid>> GetOrCreateAsync(FIO fio);
}