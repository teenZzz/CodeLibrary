using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CodeLibrary.Models;

namespace CodeLibrary.Postgres;

public class CodeLibraryDbContext : DbContext
{
    private static ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => builder.AddConsole());

    public CodeLibraryDbContext(DbContextOptions<CodeLibraryDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CodeLibraryDbContext).Assembly);
    }

    public DbSet<Book> Books => Set<Book>();
    
    public DbSet<Author> Authors => Set<Author>();
    
    public DbSet<Tag> Tags => Set<Tag>();
    
    public DbSet<Status> Statuses => Set<Status>();
    
    public DbSet<BookAuthor> BookAuthors => Set<BookAuthor>();
    
    public DbSet<BookTag> BookTags => Set<BookTag>();
}