using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.API.DataAccess.SQLServer.Migrations
{
    /// <inheritdoc />
    public partial class AddCurriculumVitae : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Uri = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurriculumVitae",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LinkedInProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WebsiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    About = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Skills = table.Column<string>(type: "nvarchar(611)", maxLength: 611, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurriculumVitae", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurriculumVitae_Links_LinkedInProfileId",
                        column: x => x.LinkedInProfileId,
                        principalTable: "Links",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CurriculumVitae_Links_WebsiteId",
                        column: x => x.WebsiteId,
                        principalTable: "Links",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EducationHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DegreeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CurriculumVitaeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationHistory_CurriculumVitae_CurriculumVitaeId",
                        column: x => x.CurriculumVitaeId,
                        principalTable: "CurriculumVitae",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PersonalProjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CurriculumVitaeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExternalLinkId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalProjects_CurriculumVitae_CurriculumVitaeId",
                        column: x => x.CurriculumVitaeId,
                        principalTable: "CurriculumVitae",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PersonalProjects_Links_ExternalLinkId",
                        column: x => x.ExternalLinkId,
                        principalTable: "Links",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkExperiences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EmploymentType = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LocationType = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CurriculumVitaeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExternalLinkId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkExperiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkExperiences_CurriculumVitae_CurriculumVitaeId",
                        column: x => x.CurriculumVitaeId,
                        principalTable: "CurriculumVitae",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkExperiences_Links_ExternalLinkId",
                        column: x => x.ExternalLinkId,
                        principalTable: "Links",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "CurriculumVitae",
                columns: new[] { "Id", "About", "Email", "LinkedInProfileId", "Location", "Name", "Phone", "Skills", "WebsiteId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), null, "email@email", null, "Location", "Name", "(000) 000-0000", "", null });

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumVitae_LinkedInProfileId",
                table: "CurriculumVitae",
                column: "LinkedInProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumVitae_WebsiteId",
                table: "CurriculumVitae",
                column: "WebsiteId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationHistory_CurriculumVitaeId",
                table: "EducationHistory",
                column: "CurriculumVitaeId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalProjects_CurriculumVitaeId",
                table: "PersonalProjects",
                column: "CurriculumVitaeId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalProjects_ExternalLinkId",
                table: "PersonalProjects",
                column: "ExternalLinkId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperiences_CurriculumVitaeId",
                table: "WorkExperiences",
                column: "CurriculumVitaeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperiences_ExternalLinkId",
                table: "WorkExperiences",
                column: "ExternalLinkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EducationHistory");

            migrationBuilder.DropTable(
                name: "PersonalProjects");

            migrationBuilder.DropTable(
                name: "WorkExperiences");

            migrationBuilder.DropTable(
                name: "CurriculumVitae");

            migrationBuilder.DropTable(
                name: "Links");
        }
    }
}
