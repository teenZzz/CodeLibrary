namespace CodeLibrary.UseCases.InterfacesRepositories;

public interface ILoadQueries
{
    Task<List<string>> GetAllTagsAsync();
    Task<List<string>> GetAllStatusesAsync();
}