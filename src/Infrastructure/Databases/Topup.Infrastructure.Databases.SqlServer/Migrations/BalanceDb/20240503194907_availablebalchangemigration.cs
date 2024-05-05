using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Topup.Infrastructure.Databases.SqlServer.Migrations.BalanceDb
{
    /// <inheritdoc />
    public partial class availablebalchangemigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "Users",
                newName: "CurrentBalance");

            migrationBuilder.AddColumn<decimal>(
                name: "AvailableBalance",
                table: "Users",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Users",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableBalance",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "CurrentBalance",
                table: "Users",
                newName: "Balance");
        }
    }
}
