﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Organization.Infrastructure.Data;

#nullable disable

namespace Organization.Infrastructure.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("multi_tenant")
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Organization.Domain.Entities.Organization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INT")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("VARCHAR(50)")
                        .HasColumnName("name");

                    b.Property<string>("SlugTenant")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("slug_tenant");

                    b.HasKey("Id");

                    b.HasIndex("SlugTenant", "Name")
                        .IsUnique();

                    b.ToTable("organization", "multi_tenant");
                });

            modelBuilder.Entity("Organization.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INT")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("email");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("password");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("user", "multi_tenant");
                });

            modelBuilder.Entity("Organization.Domain.Entities.UserOrganization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INT")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("OrganizationId")
                        .HasColumnType("INT")
                        .HasColumnName("organization_id");

                    b.Property<int>("UserId")
                        .HasColumnType("INT")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("OrganizationId", "UserId")
                        .IsUnique();

                    b.ToTable("user_organization", "multi_tenant");
                });

            modelBuilder.Entity("Organization.Domain.Entities.UserOrganization", b =>
                {
                    b.HasOne("Organization.Domain.Entities.Organization", "Organization")
                        .WithMany("UserOrganization")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Organization.Domain.Entities.User", "User")
                        .WithMany("UserOrganization")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Organization.Domain.Entities.Organization", b =>
                {
                    b.Navigation("UserOrganization");
                });

            modelBuilder.Entity("Organization.Domain.Entities.User", b =>
                {
                    b.Navigation("UserOrganization");
                });
#pragma warning restore 612, 618
        }
    }
}
