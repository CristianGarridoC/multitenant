using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Product.Infrastructure.Data.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Domain.Entities.Product>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Product> builder)
    {
        builder
            .ToTable("product");
        
        builder
            .HasKey(d => d.Id);
        
        builder
            .HasIndex(d => d.Name)
            .IsUnique();
        
        builder
            .Property(d => d.Id)
            .HasColumnName("id")
            .HasColumnType("INT");
        
        builder
            .Property(d => d.Name)
            .HasColumnName("name")
            .HasColumnType("VARCHAR(100)")
            .IsRequired();
        
        builder
            .Property(d => d.Description)
            .HasColumnName("description")
            .HasColumnType("TEXT")
            .IsRequired();
        
        builder
            .Property(d => d.Duration)
            .HasColumnName("duration")
            .HasColumnType("INT");
    }
}