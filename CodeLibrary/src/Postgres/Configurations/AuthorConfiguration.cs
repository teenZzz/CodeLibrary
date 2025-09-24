using CodeLibrary.Common;
using CodeLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeLibrary.Postgres.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        ConfigureTable(builder);
        ConfigureProperties(builder);
        ConfigureValueObjects(builder);
    }

    private static void ConfigureTable(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("authors");
        builder.HasKey(a => a.Id).HasName("pk_authors");
    }

    private static void ConfigureProperties(EntityTypeBuilder<Author> builder)
    {
        builder.Property(a => a.Id)
            .IsRequired()
            .HasColumnName("id");
    }

    private static void ConfigureValueObjects(EntityTypeBuilder<Author> builder)
    {
        builder.OwnsOne(a => a.Fio, ab =>
        {
            ab.Property(a => a.Surname)
                .HasMaxLength(Consts.Text.MAX_LENGTH)
                .IsRequired()
                .HasColumnName("surname");

            ab.Property(a => a.FirstName)
                .HasMaxLength(Consts.Text.MAX_LENGTH)
                .IsRequired()
                .HasColumnName("first_name");

            ab.Property(a => a.Patronymic)
                .IsRequired(false)
                .HasColumnName("patronymic");
        });

        builder.Navigation(a => a.Fio).IsRequired();
    }
}