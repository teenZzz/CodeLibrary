using CodeLibrary.Common;
using CodeLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeLibrary.Postgres.Configurations;

public class StatusConfiguration : IEntityTypeConfiguration<Status>
{
    public void Configure(EntityTypeBuilder<Status> builder)
    {
        ConfigureTable(builder);
        ConfigureProperty(builder);
        ConfigureValueObjects(builder);
    }

    private static void ConfigureTable(EntityTypeBuilder<Status> builder)
    {
        builder.ToTable("statuses");
        builder.HasKey(s => s.Id).HasName("pk_statuses");
    }

    private static void ConfigureProperty(EntityTypeBuilder<Status> builder)
    {
        builder.Property(s => s.Id)
            .IsRequired()
            .HasColumnName("id");
    }

    private static void ConfigureValueObjects(EntityTypeBuilder<Status> builder)
    {
        builder.OwnsOne(s => s.Name, sb =>
        {
            sb.Property(s => s.Value)
                .HasMaxLength(Consts.Text.MAX_LENGTH)
                .HasColumnName("name");
        });

        builder.Navigation(s => s.Name).IsRequired();
    }
}