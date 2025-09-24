using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CodeLibrary.Postgres;

public class CodeLibraryDbContextFactory : IDesignTimeDbContextFactory<CodeLibraryDbContext>
{
    public CodeLibraryDbContext CreateDbContext(string[] args)
    {
        // Берём конфиг из стартового проекта (обычно из каталога, где ты вызываешь dotnet ef)
        var basePath = Directory.GetCurrentDirectory();

        var config = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)   // можно и env переменные добавить
            .AddEnvironmentVariables()
            .Build();

        var cs = config.GetConnectionString("Postgres")
                 ?? "Host=localhost;Port=5434;Database=code_library_db;Username=postgres;Password=postgres";

        var loggerFactory = LoggerFactory.Create(b => b.AddConsole());

        var options = new DbContextOptionsBuilder<CodeLibraryDbContext>()
            .UseNpgsql(cs)
            .UseLoggerFactory(loggerFactory)
            .Options;

        return new CodeLibraryDbContext(options);
    }
}