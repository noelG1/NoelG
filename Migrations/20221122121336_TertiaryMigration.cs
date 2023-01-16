using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankManagementSystemApi.Migrations
{
    /// <inheritdoc />
    public partial class TertiaryMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "phoneNumber",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "address",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "phoneNumber",
                table: "Customers");
        }
    }
}
