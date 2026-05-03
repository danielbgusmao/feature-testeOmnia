using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Configures the mapping for the SaleItem entity to the SaleItems table.
/// Defines column properties, relationships, and constraints.
/// </summary>
public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        // Primary Key
        builder.HasKey(si => si.Id);
        builder.Property(si => si.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        // Properties
        builder.Property(si => si.SaleId)
            .IsRequired()
            .HasColumnType("uuid");

        builder.Property(si => si.ProductId)
            .IsRequired()
            .HasColumnType("uuid");

        builder.Property(si => si.ProductName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(si => si.Quantity)
            .IsRequired();

        builder.Property(si => si.UnitPrice)
            .IsRequired()
            .HasColumnType("numeric(18,2)");

        builder.Property(si => si.DiscountPercentage)
            .IsRequired()
            .HasColumnType("numeric(5,2)");

        builder.Property(si => si.DiscountAmount)
            .IsRequired()
            .HasColumnType("numeric(18,2)");

        builder.Property(si => si.TotalAmount)
            .IsRequired()
            .HasColumnType("numeric(18,2)");

        builder.Property(si => si.IsCancelled)
            .IsRequired();

        builder.Property(si => si.CreatedAt)
            .IsRequired();

        builder.Property(si => si.UpdatedAt);

        builder.Property(si => si.CancelledAt);

        // Indexes
        builder.HasIndex(si => si.SaleId);
        builder.HasIndex(si => si.ProductId);
    }
}
