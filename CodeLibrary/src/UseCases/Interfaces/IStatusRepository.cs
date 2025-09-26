using CodeLibrary.Models;

namespace CodeLibrary.UseCases.InterfacesRepositories;

public interface IStatusRepository
{
    Task<Status?> GetByNameAsync(string name);
}