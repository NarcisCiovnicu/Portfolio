﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Portfolio.API.DataAccess;

#nullable disable

namespace Portfolio.API.DataAccess.SQLServer.Migrations
{
    [DbContext(typeof(PortfolioDbContext))]
    partial class PortfolioDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Portfolio.API.DataAccess.Entities.ApiTracker", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(0);

                    b.Property<string>("City")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("Country")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("InternetProvider")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("IpLocationError")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<bool?>("IsMobile")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsProxy")
                        .HasColumnType("bit");

                    b.Property<double?>("Latitude")
                        .HasColumnType("float");

                    b.Property<double?>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("RoutePath")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserAgent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZipCode")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.HasKey("Id");

                    b.ToTable("Tracking");
                });

            modelBuilder.Entity("Portfolio.API.DataAccess.Entities.CurriculumVitae", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(0);

                    b.Property<string>("About")
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("LinkedInProfileId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Skills")
                        .IsRequired()
                        .HasMaxLength(611)
                        .HasColumnType("nvarchar(611)");

                    b.Property<Guid?>("WebsiteId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("LinkedInProfileId");

                    b.HasIndex("WebsiteId");

                    b.ToTable("CurriculumVitae");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000001"),
                            Email = "email@email",
                            Location = "Location",
                            Name = "Name",
                            Phone = "(000) 000-0000",
                            Skills = ""
                        });
                });

            modelBuilder.Entity("Portfolio.API.DataAccess.Entities.Education", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(0);

                    b.Property<Guid?>("CurriculumVitaeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DegreeName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("date");

                    b.Property<string>("SchoolName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.HasIndex("CurriculumVitaeId");

                    b.ToTable("EducationHistory");
                });

            modelBuilder.Entity("Portfolio.API.DataAccess.Entities.Link", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(0);

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.HasKey("Id");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("Portfolio.API.DataAccess.Entities.Password", b =>
                {
                    b.Property<string>("HashValue")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("HashValue");

                    b.ToTable("Passwords");
                });

            modelBuilder.Entity("Portfolio.API.DataAccess.Entities.PersonalProject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(0);

                    b.Property<Guid?>("CurriculumVitaeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<Guid?>("ExternalLinkId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CurriculumVitaeId");

                    b.HasIndex("ExternalLinkId");

                    b.ToTable("PersonalProjects");
                });

            modelBuilder.Entity("Portfolio.API.DataAccess.Entities.WorkExperience", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(0);

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("CurriculumVitaeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<string>("EmploymentType")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<DateOnly?>("EndDate")
                        .HasColumnType("date");

                    b.Property<Guid?>("ExternalLinkId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Location")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("LocationType")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CurriculumVitaeId");

                    b.HasIndex("ExternalLinkId");

                    b.ToTable("WorkExperiences");
                });

            modelBuilder.Entity("Portfolio.API.DataAccess.Entities.CurriculumVitae", b =>
                {
                    b.HasOne("Portfolio.API.DataAccess.Entities.Link", "LinkedInProfile")
                        .WithMany()
                        .HasForeignKey("LinkedInProfileId");

                    b.HasOne("Portfolio.API.DataAccess.Entities.Link", "Website")
                        .WithMany()
                        .HasForeignKey("WebsiteId");

                    b.Navigation("LinkedInProfile");

                    b.Navigation("Website");
                });

            modelBuilder.Entity("Portfolio.API.DataAccess.Entities.Education", b =>
                {
                    b.HasOne("Portfolio.API.DataAccess.Entities.CurriculumVitae", null)
                        .WithMany("EducationHistory")
                        .HasForeignKey("CurriculumVitaeId");
                });

            modelBuilder.Entity("Portfolio.API.DataAccess.Entities.PersonalProject", b =>
                {
                    b.HasOne("Portfolio.API.DataAccess.Entities.CurriculumVitae", null)
                        .WithMany("PersonalProjects")
                        .HasForeignKey("CurriculumVitaeId");

                    b.HasOne("Portfolio.API.DataAccess.Entities.Link", "ExternalLink")
                        .WithMany()
                        .HasForeignKey("ExternalLinkId");

                    b.Navigation("ExternalLink");
                });

            modelBuilder.Entity("Portfolio.API.DataAccess.Entities.WorkExperience", b =>
                {
                    b.HasOne("Portfolio.API.DataAccess.Entities.CurriculumVitae", null)
                        .WithMany("WorkExperienceList")
                        .HasForeignKey("CurriculumVitaeId");

                    b.HasOne("Portfolio.API.DataAccess.Entities.Link", "ExternalLink")
                        .WithMany()
                        .HasForeignKey("ExternalLinkId");

                    b.Navigation("ExternalLink");
                });

            modelBuilder.Entity("Portfolio.API.DataAccess.Entities.CurriculumVitae", b =>
                {
                    b.Navigation("EducationHistory");

                    b.Navigation("PersonalProjects");

                    b.Navigation("WorkExperienceList");
                });
#pragma warning restore 612, 618
        }
    }
}
