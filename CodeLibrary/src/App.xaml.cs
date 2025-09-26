using System.Configuration;
using System.Data;
using System.Windows;
using CodeLibrary.Common;
using CodeLibrary.Postgres;
using CodeLibrary.Postgres.Repositories;
using CodeLibrary.UseCases.Handlers;
using CodeLibrary.UseCases.InterfacesRepositories;
using CodeLibrary.ViewModels;
using CodeLibrary.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace CodeLibrary;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;
    
    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var services = new ServiceCollection();
        
        // Добавляем логирование
        services.AddLogging(builder =>
        {
            builder.AddConsole();
        });

        var cs = config.GetConnectionString("Postgres");

        // DbContext
        services.AddDbContext<CodeLibraryDbContext>(opt =>
            opt.UseNpgsql(cs));

        // Репозитории
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IStatusRepository, StatusRepository>();

        // Хэндлеры/UseCases
        services.AddScoped<CreateAuthorHandler>();
        services.AddScoped<CreateBookHandler>();
        
        // VM и окно (если используешь DI для UI)
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<MainWindow>();
        
        // Собираем провайдер
        Services = services.BuildServiceProvider();
        
        using (var scope = Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<CodeLibraryDbContext>();
            db.Database.Migrate();
            
            await DbSeeder.SeedAsync(db);
        }
        
        // Показываем главное окно
        var window = Services.GetRequiredService<MainWindow>();
        window.DataContext = Services.GetRequiredService<MainWindowViewModel>();
        window.Show();
    }
}