using CodeLibrary.Models.DTOs;
using CodeLibrary.Postgres;
using CodeLibrary.UseCases.InterfacesRepositories;
using Microsoft.EntityFrameworkCore;

namespace CodeLibrary.UseCases.Queries;

public class BookQueries : IBookQueries
{
    private readonly CodeLibraryDbContext _db;

    public BookQueries(CodeLibraryDbContext db)
    {
        _db = db;
    } 
    
    public async Task<List<BookDto>> GetAllAsync()
    {
        var rows = await _db.Books
            .AsNoTracking()
            .Select(b => new
            {
                Id = b.Id,
                Title = b.Name.Value,
                Description = b.Description != null ? b.Description.Value : null,
                Status = _db.Statuses
                    .Where(s => s.Id == b.StatusId)
                    .Select(s => s.Name.Value)
                    .FirstOrDefault(),

                Authors = _db.BookAuthors
                    .Where(ba => ba.BookId == b.Id)
                    .Join(_db.Authors,
                          ba => ba.AuthorId,
                          a  => a.Id,
                          (ba, a) => a.Fio.Surname + " " + a.Fio.FirstName +
                                     (a.Fio.Patronymic != null ? " " + a.Fio.Patronymic : ""))
                    .ToList(),

                Tags = _db.BookTags
                    .Where(bt => bt.BookId == b.Id)
                    .Join(_db.Tags,
                          bt => bt.TagId,
                          t  => t.Id,
                          (bt, t) => t.Name.Value)
                    .ToList()
            })
            .ToListAsync();

        return rows.Select(r => new BookDto
        {
            Id = r.Id,
            Title = r.Title,
            Author = string.Join(", ", r.Authors),
            Tag = string.Join(", ", r.Tags),
            Description = r.Description,
            Status = r.Status ?? ""
        }).ToList();
    }

    public async Task<BookDto?> GetByIdAsync(Guid id)
    {
        var row = await _db.Books
            .AsNoTracking()
            .Where(b => b.Id == id)
            .Select(b => new
            {
                Id = b.Id,
                Title = b.Name.Value,
                Description = b.Description != null ? b.Description.Value : null,
                Status = _db.Statuses
                    .Where(s => s.Id == b.StatusId)
                    .Select(s => s.Name.Value)
                    .FirstOrDefault(),

                Authors = _db.BookAuthors
                    .Where(ba => ba.BookId == b.Id)
                    .Join(_db.Authors,
                          ba => ba.AuthorId,
                          a  => a.Id,
                          (ba, a) => a.Fio.Surname + " " + a.Fio.FirstName +
                                     (a.Fio.Patronymic != null ? " " + a.Fio.Patronymic : ""))
                    .ToList(),

                Tags = _db.BookTags
                    .Where(bt => bt.BookId == b.Id)
                    .Join(_db.Tags,
                          bt => bt.TagId,
                          t  => t.Id,
                          (bt, t) => t.Name.Value)
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (row is null) return null;

        return new BookDto
        {
            Id = row.Id,
            Title = row.Title,
            Author = string.Join(", ", row.Authors),
            Tag = string.Join(", ", row.Tags),
            Description = row.Description,
            Status = row.Status ?? ""
        };
    }
}