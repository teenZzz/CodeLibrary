using CodeLibrary.Models;
using CodeLibrary.UseCases.InterfacesRepositories;
using Microsoft.EntityFrameworkCore;

namespace CodeLibrary.Postgres.Repositories;

public class TagRepository : ITagRepository
{
    private readonly CodeLibraryDbContext _db;
    public TagRepository(CodeLibraryDbContext db) => _db = db;

    public Task<Tag?> GetByNameAsync(string name) =>
        _db.Tags.AsNoTracking().FirstOrDefaultAsync(t => t.Name.Value == name);
}