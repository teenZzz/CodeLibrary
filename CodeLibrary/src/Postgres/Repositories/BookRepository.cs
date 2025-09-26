using System.Globalization;
using CodeLibrary.Models;
using CodeLibrary.Models.DTOs;
using CodeLibrary.UseCases.InterfacesRepositories;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;

namespace CodeLibrary.Postgres.Repositories;

public class BookRepository : IBookRepository
{
    private readonly CodeLibraryDbContext _dbContext;
    private readonly ILogger<BookRepository> _logger;
    
    public BookRepository(CodeLibraryDbContext dbContext, ILogger<BookRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<Result<Guid>> Add(Book book)
    {
        try
        {
            await _dbContext.Books.AddAsync(book);

            await _dbContext.SaveChangesAsync();

            return book.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to insert location");

            return Result.Failure<Guid>(ex.Message);
        }
    }
}