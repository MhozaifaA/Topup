using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Topup.Infrastructure.Databases.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class indexconstrainonbothben : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Beneficiaries_UserId",
                table: "Beneficiaries");

            migrationBuilder.DeleteData(
                table: "Options",
                keyColumn: "Id",
                keyValue: new Guid("0ff45bfc-6fd3-4204-aa96-e1fe8d45fabf"));

            migrationBuilder.DeleteData(
                table: "Options",
                keyColumn: "Id",
                keyValue: new Guid("13add3f3-d04e-4d56-8469-1eee4901e5f0"));

            migrationBuilder.DeleteData(
                table: "Options",
                keyColumn: "Id",
                keyValue: new Guid("1c969eb7-cf9c-46ce-9bf4-36e7c4d93357"));

            migrationBuilder.DeleteData(
                table: "Options",
                keyColumn: "Id",
                keyValue: new Guid("4701e584-ce65-4d3e-ba76-80babee240e3"));

            migrationBuilder.DeleteData(
                table: "Options",
                keyColumn: "Id",
                keyValue: new Guid("58c166c0-ffad-439f-8a6e-356e8727d1c2"));

            migrationBuilder.DeleteData(
                table: "Options",
                keyColumn: "Id",
                keyValue: new Guid("9174454c-92db-4bcf-8e24-ce345d97dffd"));

            migrationBuilder.DeleteData(
                table: "Options",
                keyColumn: "Id",
                keyValue: new Guid("b15297cf-6bd6-42f1-919b-7b3f7f3d8364"));

            migrationBuilder.InsertData(
                table: "Options",
                columns: new[] { "Id", "CreatedBy", "DateCreated", "DateDeleted", "DateUpdated", "DeletedBy", "Name", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("1df82868-0d58-455d-9d94-074349365d1c"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "AED 75", null },
                    { new Guid("4349fe4e-f6b4-40b7-a0d3-073a63a7eeae"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "AED 20", null },
                    { new Guid("501e0cee-dcc6-4a31-8e3a-f1e6276caf5b"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "AED 30", null },
                    { new Guid("622f1223-b7a0-4962-8f53-3a62260aa929"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "AED 10", null },
                    { new Guid("9eaa0d82-c872-48f0-93f7-5a1649c590f6"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "AED 5", null },
                    { new Guid("ef62eec1-cb54-45bb-9177-accb706ab266"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "AED 50", null },
                    { new Guid("f1f7cbb2-6bac-463f-b8af-1aef0ac27614"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "AED 100", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Beneficiaries_UserId_Name",
                table: "Beneficiaries",
                columns: new[] { "UserId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Beneficiaries_UserId_Name",
                table: "Beneficiaries");

            migrationBuilder.DeleteData(
                table: "Options",
                keyColumn: "Id",
                keyValue: new Guid("1df82868-0d58-455d-9d94-074349365d1c"));

            migrationBuilder.DeleteData(
                table: "Options",
                keyColumn: "Id",
                keyValue: new Guid("4349fe4e-f6b4-40b7-a0d3-073a63a7eeae"));

            migrationBuilder.DeleteData(
                table: "Options",
                keyColumn: "Id",
                keyValue: new Guid("501e0cee-dcc6-4a31-8e3a-f1e6276caf5b"));

            migrationBuilder.DeleteData(
                table: "Options",
                keyColumn: "Id",
                keyValue: new Guid("622f1223-b7a0-4962-8f53-3a62260aa929"));

            migrationBuilder.DeleteData(
                table: "Options",
                keyColumn: "Id",
                keyValue: new Guid("9eaa0d82-c872-48f0-93f7-5a1649c590f6"));

            migrationBuilder.DeleteData(
                table: "Options",
                keyColumn: "Id",
                keyValue: new Guid("ef62eec1-cb54-45bb-9177-accb706ab266"));

            migrationBuilder.DeleteData(
                table: "Options",
                keyColumn: "Id",
                keyValue: new Guid("f1f7cbb2-6bac-463f-b8af-1aef0ac27614"));

            migrationBuilder.InsertData(
                table: "Options",
                columns: new[] { "Id", "CreatedBy", "DateCreated", "DateDeleted", "DateUpdated", "DeletedBy", "Name", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("0ff45bfc-6fd3-4204-aa96-e1fe8d45fabf"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "AED 5", null },
                    { new Guid("13add3f3-d04e-4d56-8469-1eee4901e5f0"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "AED 100", null },
                    { new Guid("1c969eb7-cf9c-46ce-9bf4-36e7c4d93357"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "AED 10", null },
                    { new Guid("4701e584-ce65-4d3e-ba76-80babee240e3"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "AED 75", null },
                    { new Guid("58c166c0-ffad-439f-8a6e-356e8727d1c2"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "AED 20", null },
                    { new Guid("9174454c-92db-4bcf-8e24-ce345d97dffd"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "AED 50", null },
                    { new Guid("b15297cf-6bd6-42f1-919b-7b3f7f3d8364"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "AED 30", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Beneficiaries_UserId",
                table: "Beneficiaries",
                column: "UserId");
        }
    }
}
