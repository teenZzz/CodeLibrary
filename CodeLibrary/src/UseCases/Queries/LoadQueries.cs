using CodeLibrary.Postgres;
using CodeLibrary.UseCases.InterfacesRepositories;
using Microsoft.EntityFrameworkCore;

namespace CodeLibrary.UseCases.Queries;

public class LoadQueries : ILoadQueries
{
    private readonly CodeLibraryDbContext _db;

    public LoadQueries(CodeLibraryDbContext db)
    {
        _db = db;
    }

    public async Task<List<string>> GetAllTagsAsync()
    {
        var tags = await _db.Tags
            .AsNoTracking()
            .Select(t => t.Name.Value)   // предполагаю, что поле называется Name.Value, как в BookDto
            .ToListAsync();

        return tags;
    }

    public async Task<List<string>> GetAllStatusesAsync()
    {
        var statuses = await _db.Statuses
            .AsNoTracking()
            .Select(s => s.Name.Value)
            .ToListAsync();

        return statuses;
    }
}