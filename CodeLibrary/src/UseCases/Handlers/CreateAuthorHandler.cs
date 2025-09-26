using CodeLibrary.Models;
using CodeLibrary.Models.Requests;
using CodeLibrary.Models.ValueObjects;
using CodeLibrary.UseCases.InterfacesRepositories;
using CSharpFunctionalExtensions;

namespace CodeLibrary.UseCases.Handlers;

public class CreateAuthorHandler
{
    private readonly IAuthorRepository _authorRepository;
    
    public CreateAuthorHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }
    
    public async Task<Result<Guid>> Handle(CreateAuthorRequest request)
    {
        var fioResult = FIO.Create(request.FioDto.Surname, request.FioDto.FirstName, request.FioDto.Patronymic);

        if (fioResult.IsFailure)
            return Result.Failure<Guid>(fioResult.Error);

        var fio = fioResult.Value;

        var author = Author.Create(fio).Value;
        
        // Сохранение доменных моделей в БД
        await _authorRepository.Add(author);

        return author.Id;
    }
}