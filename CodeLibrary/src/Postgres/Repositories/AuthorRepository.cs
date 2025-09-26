using CodeLibrary.Models;
using CodeLibrary.Models.ValueObjects;
using CodeLibrary.UseCases.InterfacesRepositories;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeLibrary.Postgres.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private readonly CodeLibraryDbContext _dbContext;
    private readonly ILogger<AuthorRepository> _logger;
    
    public AuthorRepository(CodeLibraryDbContext dbContext, ILogger<AuthorRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<Result<Guid>> Add(Author author)
    {
        try
        {
            await _dbContext.Authors.AddAsync(author);

            await _dbContext.SaveChangesAsync();

            return author.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to insert location");

            return Result.Failure<Guid>(ex.Message);
        }
    }

    public async Task<Author?> FindByFioAsync(FIO fio)
    {
        return await _dbContext.Authors
            .AsNoTracking()
            .FirstOrDefaultAsync(a =>
                a.Fio.Surname == fio.Surname &&
                a.Fio.FirstName == fio.FirstName &&
                a.Fio.Patronymic == fio.Patronymic);
    }

    public async Task<Result<Guid>> GetOrCreateAsync(FIO fio)
    {
        try
        {
            // Проверка
            var existing = await FindByFioAsync(fio);
            if (existing is not null)
                return existing.Id;

            // Создание
            var authorResult = Author.Create(fio);
            if (authorResult.IsFailure)
                return Result.Failure<Guid>(authorResult.Error);

            var author = authorResult.Value;
            await _dbContext.Authors.AddAsync(author);
            await _dbContext.SaveChangesAsync();

            return author.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to get or create author");
            return Result.Failure<Guid>(ex.Message);
        }
    }
}