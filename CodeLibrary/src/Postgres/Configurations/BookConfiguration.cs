using CodeLibrary.Common;
using CodeLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeLibrary.Postgres.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        ConfigureTable(builder);
        ConfigureProperties(builder);
        ConfigureValueObjects(builder);
        ConfigureRelations(builder);
    }
    
    private static void ConfigureTable(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("books");
        builder.HasKey(b => b.Id).HasName("pk_books");
    }

    private static void ConfigureProperties(EntityTypeBuilder<Book> builder)
    {
        builder.Property(b => b.Id)
            .IsRequired()
            .HasColumnName("id");

        builder.Property(b => b.StatusId)
            .IsRequired()
            .HasColumnName("status_id");
    }
    
    private static void ConfigureValueObjects(EntityTypeBuilder<Book> builder)
    {
        ConfigureBookName(builder);
        ConfigureBookDescription(builder);
    }

    private static void ConfigureBookName(EntityTypeBuilder<Book> builder)
    {
        builder.OwnsOne(b => b.Name, nb =>
        {
            nb.Property(b => b.Value)
                .IsRequired()
                .HasMaxLength(Consts.Text.MAX_LENGTH)
                .HasColumnName("name");
        });

        builder.Navigation(b => b.Name).IsRequired();
    }
    
    private static void ConfigureBookDescription(EntityTypeBuilder<Book> builder)
    {
        builder.OwnsOne(b => b.Description, db =>
        {
            db.Property(b => b.Value)
                .IsRequired(false)
                .HasMaxLength(Consts.Text.MAX_LENGTH)
                .HasColumnName("description");
        });

        builder.Navigation(b => b.Description).IsRequired(false);
    }

    private static void ConfigureRelations(EntityTypeBuilder<Book> builder)
    {
        ConfigureAuthorRelation(builder);
        ConfigureTagRelation(builder);
        ConfigureStatusRelation(builder);
    }

    private static void ConfigureAuthorRelation(EntityTypeBuilder<Book> builder)
    {
        builder.HasMany(b => b.BookAuthors)
            .WithOne()
            .HasForeignKey(bp => bp.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(b => b.BookAuthors)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_bookAuthors");
    }

    private static void ConfigureTagRelation(EntityTypeBuilder<Book> builder)
    {
        builder.HasMany(b => b.BookTags)
            .WithOne()
            .HasForeignKey(bp => bp.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(b => b.BookTags)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_bookTags");
    }

    private static void ConfigureStatusRelation(EntityTypeBuilder<Book> builder)
    {
        builder.HasOne<Status>()                 
            .WithMany()                       
            .HasForeignKey(b => b.StatusId)   
            .HasConstraintName("fk_books_statuses") 
            .OnDelete(DeleteBehavior.Restrict);
    }
}