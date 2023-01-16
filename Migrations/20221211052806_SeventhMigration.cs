using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankManagementSystemApi.Migrations
{
    /// <inheritdoc />
    public partial class SeventhMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "age",
                table: "CustomerServiceRepresentatives");

            migrationBuilder.DropColumn(
                name: "age",
                table: "Customers");

            migrationBuilder.AddColumn<DateTime>(
                name: "birthDate",
                table: "CustomerServiceRepresentatives",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "CustomerServiceRepresentatives",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "birthDate",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "birthDate",
                table: "CustomerServiceRepresentatives");

            migrationBuilder.DropColumn(
                name: "city",
                table: "CustomerServiceRepresentatives");

            migrationBuilder.DropColumn(
                name: "birthDate",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "city",
                table: "Customers");

            migrationBuilder.AddColumn<int>(
                name: "age",
                table: "CustomerServiceRepresentatives",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "age",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
