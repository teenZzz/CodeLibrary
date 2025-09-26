using CodeLibrary.Models;
using CodeLibrary.Models.ValueObjects;
using CodeLibrary.Postgres;
using Microsoft.EntityFrameworkCore;

namespace CodeLibrary.Common;

public static class DbSeeder
{
    public static async Task SeedAsync(CodeLibraryDbContext db)
    {
        // Тэги
        var tagNames = new[] { "C#", "Паттерны", "Архитектура", "Написание кода" };
        foreach (var tagName in tagNames)
        {
            if (!await db.Tags.AnyAsync(t => t.Name.Value == tagName))
            {
                var name = Name.Create(tagName).Value;
                var tag  = Tag.Create(name).Value;
                await db.Tags.AddAsync(tag);
            }
        }

        // Статусы
        var statusNames = new[] { "В планах", "Прочитано", "Читаю" };
        foreach (var statusName in statusNames)
        {
            if (!await db.Statuses.AnyAsync(s => s.Name.Value == statusName))
            {
                var name   = Name.Create(statusName).Value;
                var status = Status.Create(name).Value;
                await db.Statuses.AddAsync(status);
            }
        }

        await db.SaveChangesAsync();
    }
}