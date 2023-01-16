using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankManagementSystemApi.Migrations
{
    /// <inheritdoc />
    public partial class FifthMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_customerServiceRepresentatives",
                table: "customerServiceRepresentatives");

            migrationBuilder.RenameTable(
                name: "customerServiceRepresentatives",
                newName: "CustomerServiceRepresentatives");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerServiceRepresentatives",
                table: "CustomerServiceRepresentatives",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerServiceRepresentatives",
                table: "CustomerServiceRepresentatives");

            migrationBuilder.RenameTable(
                name: "CustomerServiceRepresentatives",
                newName: "customerServiceRepresentatives");

            migrationBuilder.AddPrimaryKey(
                name: "PK_customerServiceRepresentatives",
                table: "customerServiceRepresentatives",
                column: "id");
        }
    }
}
