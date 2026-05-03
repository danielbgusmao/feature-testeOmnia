using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Configures the mapping for the Sale entity to the Sales table.
/// Defines column properties, relationships, and constraints.
/// </summary>
public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        // Primary Key
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        // Properties
        builder.Property(s => s.SaleNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.SaleDate)
            .IsRequired();

        builder.Property(s => s.CustomerId)
            .IsRequired()
            .HasColumnType("uuid");

        builder.Property(s => s.CustomerName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.BranchId)
            .IsRequired()
            .HasColumnType("uuid");

        builder.Property(s => s.BranchName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.TotalAmount)
            .IsRequired()
            .HasColumnType("numeric(18,2)");

        builder.Property(s => s.IsCancelled)
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.Property(s => s.UpdatedAt);

        builder.Property(s => s.CancelledAt);

        // Relationships
        builder.HasMany(s => s.Items)
            .WithOne()
            .HasForeignKey(si => si.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(s => s.SaleNumber)
            .IsUnique();
        builder.HasIndex(s => s.CustomerId);
        builder.HasIndex(s => s.BranchId);
    }
}
