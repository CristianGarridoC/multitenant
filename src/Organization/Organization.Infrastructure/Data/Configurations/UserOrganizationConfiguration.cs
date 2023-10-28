using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Organization.Domain.Entities;

namespace Organization.Infrastructure.Data.Configurations;

internal sealed class UserOrganizationConfiguration : IEntityTypeConfiguration<UserOrganization>
{
    public void Configure(EntityTypeBuilder<UserOrganization> builder)
    {
        builder
            .ToTable("user_organization");
        
        builder
            .HasKey(d => d.Id);
        
        builder
            .HasIndex(d => new { d.OrganizationId, d.UserId })
            .IsUnique();
        
        builder
            .Property(d => d.Id)
            .HasColumnName("id")
            .HasColumnType("INT");
        
        builder
            .Property(d => d.OrganizationId)
            .HasColumnName("organization_id")
            .HasColumnType("INT")
            .IsRequired();
        
        builder
            .Property(d => d.UserId)
            .HasColumnName("user_id")
            .HasColumnType("INT")
            .IsRequired();
        
        builder
            .HasOne(d => d.Organization)
            .WithMany(p => p.UserOrganization)
            .HasForeignKey(d => d.OrganizationId);
        
        builder
            .HasOne(d => d.User)
            .WithMany(p => p.UserOrganization)
            .HasForeignKey(d => d.UserId);
    }
}