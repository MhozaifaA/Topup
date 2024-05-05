using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Topup.Infrastructure.Databases.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class fixname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Options",
                keyColumn: "Id",
                keyValue: new Guid("0d5aba85-5f79-4da9-b5a5-fd0eb0aba025"));

            migrationBuilder.DeleteData(
                table: "Options",
                keyColumn: "Id",
                keyValue: new Guid("31150ce9-3562-498a-9e55-f574c490e948"));

            migrationBuilder.DeleteData(
                table: "Options",
                keyColumn: "Id",
                keyValue: new Guid("4b34c98e-669b-4658-9a29-b282df9971ea"));

            migrationBuilder.DeleteData(
                table: "Options",
                keyColumn: "Id",
                keyValue: new Guid("7671f900-e757-4ba2-9376-e3c02878ae3d"));

            migrationBuilder.DeleteData(
                table: "Options",
                keyColumn: "Id",
                keyValue: new Guid("cec1df39-5856-4793-bf88-1492027bb77e"));

            migrationBuilder.DeleteData(
                table: "Options",
                keyColumn: "Id",
                keyValue: new Guid("d5da711f-70c9-4623-9f40-e425e6197cd6"));

            migrationBuilder.DeleteData(
                table: "Options",
                keyColumn: "Id",
                keyValue: new Guid("e5382ce0-132d-4f52-b60f-0f291f218b5a"));

            migrationBuilder.RenameColumn(
                name: "PhoneNumbers",
                table: "Beneficiaries",
                newName: "PhoneNumber");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Beneficiaries",
                newName: "PhoneNumbers");

            migrationBuilder.InsertData(
                table: "Options",
                columns: new[] { "Id", "CreatedBy", "DateCreated", "DateDeleted", "DateUpdated", "DeletedBy", "Name", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("0d5aba85-5f79-4da9-b5a5-fd0eb0aba025"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "AED 10", null },
                    { new Guid("31150ce9-3562-498a-9e55-f574c490e948"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "AED 50", null },
                    { new Guid("4b34c98e-669b-4658-9a29-b282df9971ea"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "AED 5", null },
                    { new Guid("7671f900-e757-4ba2-9376-e3c02878ae3d"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "AED 100", null },
                    { new Guid("cec1df39-5856-4793-bf88-1492027bb77e"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "AED 30", null },
                    { new Guid("d5da711f-70c9-4623-9f40-e425e6197cd6"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "AED 20", null },
                    { new Guid("e5382ce0-132d-4f52-b60f-0f291f218b5a"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "AED 75", null }
                });
        }
    }
}
