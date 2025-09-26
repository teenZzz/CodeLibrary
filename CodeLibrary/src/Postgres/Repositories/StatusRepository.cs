using CodeLibrary.Models;
using CodeLibrary.UseCases.InterfacesRepositories;
using Microsoft.EntityFrameworkCore;

namespace CodeLibrary.Postgres.Repositories;

public class StatusRepository : IStatusRepository
{
    private readonly CodeLibraryDbContext _db;
    public StatusRepository(CodeLibraryDbContext db) => _db = db;

    public Task<Status?> GetByNameAsync(string name) =>
        _db.Statuses.AsNoTracking().FirstOrDefaultAsync(s => s.Name.Value == name);
}