using CodeLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeLibrary.Postgres.Configurations;

public class BookTagConfiguration: IEntityTypeConfiguration<BookTag>
{
    public void Configure(EntityTypeBuilder<BookTag> builder)
    {
        builder.ToTable("book_tags");
        builder.HasKey(bt => new { bt.BookId, bt.TagId }).HasName("pk_book_tags");

        builder.Property(bt => bt.BookId)
            .IsRequired()
            .HasColumnName("book_id");

        builder.Property(bt => bt.TagId)
            .IsRequired()
            .HasColumnName("tag_id");

        builder.HasOne<Tag>()
            .WithMany()
            .HasForeignKey(bt => bt.TagId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_book_tags_tag");
    }
    
}