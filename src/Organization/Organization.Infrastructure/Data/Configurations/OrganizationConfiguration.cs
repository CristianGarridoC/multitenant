using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Organization.Infrastructure.Data.Configurations;

internal sealed class OrganizationConfiguration : IEntityTypeConfiguration<Domain.Entities.Organization>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Organization> builder)
    {
        builder
            .ToTable("organization");
        
        builder
            .HasKey(d => d.Id);
        
        builder
            .HasIndex(d => new { d.SlugTenant, d.Name })
            .IsUnique();
        
        builder
            .Property(d => d.Id)
            .HasColumnName("id")
            .HasColumnType("INT");
        
        builder
            .Property(d => d.Name)
            .HasColumnName("name")
            .HasColumnType("VARCHAR(50)")
            .IsRequired();
        
        builder
            .Property(d => d.SlugTenant)
            .HasColumnName("slug_tenant")
            .HasColumnType("TEXT")
            .IsRequired();
    }
}