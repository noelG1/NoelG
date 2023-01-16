using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankManagementSystemApi.Migrations
{
    /// <inheritdoc />
    public partial class QuatrinaryMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customerServiceRepresentatives",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    picture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    firstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phoneNumber = table.Column<int>(type: "int", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    age = table.Column<int>(type: "int", nullable: false),
                    salary = table.Column<double>(type: "float", nullable: false),
                    registerDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customerServiceRepresentatives", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customerServiceRepresentatives");
        }
    }
}
