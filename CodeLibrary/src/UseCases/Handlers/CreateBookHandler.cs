using CodeLibrary.Models;
using CodeLibrary.Models.DTOs;
using CodeLibrary.Models.Requests;
using CodeLibrary.Models.ValueObjects;
using CodeLibrary.Postgres;
using CodeLibrary.UseCases.InterfacesRepositories;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace CodeLibrary.UseCases.Handlers;

public class CreateBookHandler
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IStatusRepository _statusRepository;

    public CreateBookHandler(IBookRepository bookRepository, IAuthorRepository authorRepository, ITagRepository tagRepository, IStatusRepository statusRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _tagRepository = tagRepository;
        _statusRepository = statusRepository;
    }

    public async Task<Result<Guid>> Handle(CreateBookRequest request)
    {
        //Название
        var nameResult = Name.Create(request.Name);
        if (nameResult.IsFailure)
            return Result.Failure<Guid>(nameResult.Error);

        var name = nameResult.Value;
        
        //Описание
        Description? description = null;
        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            var descResult = Description.Create(request.Description);
            if (descResult.IsFailure)
                return Result.Failure<Guid>(descResult.Error);

            description = descResult.Value;
        }
        
        //Фио автора
        var fioResult = FIO.Create(request.AuthorSurname, request.AuthorFirstName, request.AuthorPatronymic);
        if (fioResult.IsFailure)
            return Result.Failure<Guid>(fioResult.Error);
        
        var fio = fioResult.Value;
        
        // Существует-ли автор? Если нет - создается
        var authorIdResult = await _authorRepository.GetOrCreateAsync(fio);
        if (authorIdResult.IsFailure)
            return Result.Failure<Guid>(authorIdResult.Error);

        var authorId = authorIdResult.Value;
        
        // Тег: найти по имени
        var tag = await _tagRepository.GetByNameAsync(request.Tag);
        if (tag is null)
            return Result.Failure<Guid>($"Тег '{request.Tag}' не найден");
        
        // Статус: найти по имени
        var status = await _statusRepository.GetByNameAsync(request.Status);
        if (status is null)
            return Result.Failure<Guid>($"Статус '{request.Status}' не найден");
        
        // Связи (Author, Tags)
        var bookAuthors = new List<BookAuthor>
        {
            BookAuthor.Create(Guid.Empty, authorId).Value // BookId выставит EF
        };

        var bookTags = new List<BookTag>
        {
            BookTag.Create(Guid.Empty, tag.Id).Value // один тег
        };

        // Создание книги
        var bookResult = Book.Create(
            name,
            bookAuthors,
            bookTags,
            description,
            status.Id);

        if (bookResult.IsFailure)
            return Result.Failure<Guid>(bookResult.Error);

        var book = bookResult.Value;

        // Создание book
        await _bookRepository.Add(book);

        return book.Id;
    }
}