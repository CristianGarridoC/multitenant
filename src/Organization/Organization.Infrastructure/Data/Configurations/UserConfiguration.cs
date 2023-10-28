using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Organization.Domain.Entities;

namespace Organization.Infrastructure.Data.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .ToTable("user");
        
        builder
            .HasKey(d => d.Id);
        
        builder
            .HasIndex(d => d.Email)
            .IsUnique();
        
        builder
            .Property(d => d.Id)
            .HasColumnName("id")
            .HasColumnType("INT");
        
        builder
            .Property(d => d.Email)
            .HasColumnName("email")
            .HasColumnType("TEXT")
            .IsRequired();
        
        builder
            .Property(d => d.Password)
            .HasColumnName("password")
            .HasColumnType("TEXT")
            .IsRequired();
    }
}