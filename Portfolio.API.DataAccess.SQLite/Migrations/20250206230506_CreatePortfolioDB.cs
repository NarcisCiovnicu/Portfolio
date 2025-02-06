using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.API.DataAccess.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class CreatePortfolioDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tracking",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IpAddress = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    RoutePath = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserAgent = table.Column<string>(type: "TEXT", nullable: true),
                    Country = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    City = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    ZipCode = table.Column<string>(type: "TEXT", maxLength: 16, nullable: true),
                    Latitude = table.Column<double>(type: "REAL", nullable: true),
                    Longitude = table.Column<double>(type: "REAL", nullable: true),
                    InternetProvider = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    IsMobile = table.Column<bool>(type: "INTEGER", nullable: true),
                    IsProxy = table.Column<bool>(type: "INTEGER", nullable: true),
                    IpLocationError = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracking", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tracking");
        }
    }
}
