using CodeLibrary.Common;
using CodeLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeLibrary.Postgres.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        ConfigureTable(builder);
        ConfigureProperties(builder);
        ConfigureValueObjects(builder);
    }

    private static void ConfigureTable(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("tags");
        builder.HasKey(t => t.Id).HasName("pk_tags");
    }

    private static void ConfigureProperties(EntityTypeBuilder<Tag> builder)
    {
        builder.Property(a => a.Id)
            .IsRequired()
            .HasColumnName("id");
    }

    private static void ConfigureValueObjects(EntityTypeBuilder<Tag> builder)
    {
        builder.OwnsOne(a => a.Name, ab =>
        {
            ab.Property(a => a.Value)
                .HasMaxLength(Consts.Text.MAX_LENGTH)
                .HasColumnName("name");
        });

        builder.Navigation(a => a.Name).IsRequired();
    }
}