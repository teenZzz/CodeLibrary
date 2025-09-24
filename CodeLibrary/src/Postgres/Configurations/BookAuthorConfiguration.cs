using CodeLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeLibrary.Postgres.Configurations;

public class BookAuthorConfiguration : IEntityTypeConfiguration<BookAuthor>
{
    public void Configure(EntityTypeBuilder<BookAuthor> builder)
    {
        builder.ToTable("book_authors");
        builder.HasKey(ba => new { ba.BookId, ba.AuthorId }).HasName("pk_book_authors");

        builder.Property(ba => ba.BookId)
            .IsRequired()
            .HasColumnName("book_id");

        builder.Property(ba => ba.AuthorId)
            .IsRequired()
            .HasColumnName("author_id");

        builder.HasOne<Author>()
            .WithMany()
            .HasForeignKey(ba => ba.AuthorId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_book_authors_author");
    }
}