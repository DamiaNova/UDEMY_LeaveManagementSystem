using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedUserTableWithNewColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a0c89e17-6dae-4d39-b35c-0d54a8dad74e",
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "FirstName", "LastName", "SecurityStamp" },
                values: new object[] { "86cadab5-9996-4d96-8186-fa402be4481c", new DateOnly(1997, 2, 27), "Mia", "Blažeković", "6079639f-0d95-4799-91ad-5699d8651ae2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a0c89e17-6dae-4d39-b35c-0d54a8dad74e",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "32b50dd4-4b75-485f-8a27-0ce2ff63b200", "4b3d4356-23c4-4a6d-9c7c-732ba9bbbb78" });
        }
    }
}
