using CodeLibrary.Models;

namespace CodeLibrary.UseCases.InterfacesRepositories;

public interface ITagRepository
{
    Task<Tag?> GetByNameAsync(string name);
}