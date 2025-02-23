using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeaveManagementSystem.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingUserRolesAndIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "42a465c4-e747-4314-8c06-e7f0d0efff98", null, "Supervisor", "SUPERVISOR" },
                    { "6b64a0a3-b68f-4222-9244-4e70dd5a681d", null, "Employee", "EMPLOYEE" },
                    { "939a4b2f-fa07-46c3-b7d1-538e247a5055", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "a0c89e17-6dae-4d39-b35c-0d54a8dad74e", 0, "32b50dd4-4b75-485f-8a27-0ce2ff63b200", "admin@localhost.com", true, false, null, "ADMIN@LOCALHOST.COM", "ADMIN@LOCALHOST.COM", "AQAAAAIAAYagAAAAEEr1h1mY9FjUylM1az+zXrrcHcPZ5g/NC/6j1smENaJgPqIjH1KPjoU0MepWy9lHxA==", null, false, "4b3d4356-23c4-4a6d-9c7c-732ba9bbbb78", false, "admin@localhost.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "939a4b2f-fa07-46c3-b7d1-538e247a5055", "a0c89e17-6dae-4d39-b35c-0d54a8dad74e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "42a465c4-e747-4314-8c06-e7f0d0efff98");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b64a0a3-b68f-4222-9244-4e70dd5a681d");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "939a4b2f-fa07-46c3-b7d1-538e247a5055", "a0c89e17-6dae-4d39-b35c-0d54a8dad74e" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "939a4b2f-fa07-46c3-b7d1-538e247a5055");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a0c89e17-6dae-4d39-b35c-0d54a8dad74e");
        }
    }
}
