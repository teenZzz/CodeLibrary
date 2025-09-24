using System.Configuration;
using System.Data;
using System.Windows;
using CodeLibrary.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace CodeLibrary;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var services = new ServiceCollection();

        var cs = config.GetConnectionString("Postgres");

        services.AddDbContext<CodeLibraryDbContext>(opt =>
            opt.UseNpgsql(cs));
    }
}