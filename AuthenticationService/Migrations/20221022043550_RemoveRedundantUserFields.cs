using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationService.Migrations;

public partial class RemoveRedundantUserFields : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Email",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "GivenName",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "Role",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "Surname",
            table: "Users");

        migrationBuilder.InsertData(
            table: "Roles",
            columns: new[] { "Id", "Role" },
            values: new object[] { 1, "Administrator" });

        migrationBuilder.InsertData(
            table: "Roles",
            columns: new[] { "Id", "Role" },
            values: new object[] { 2, "User" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Email",
            table: "Users",
            type: "text",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "GivenName",
            table: "Users",
            type: "text",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Role",
            table: "Users",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "Surname",
            table: "Users",
            type: "text",
            nullable: true);

        migrationBuilder.DeleteData(
            table: "Roles",
            keyColumn: "Id",
            keyValue: 1);

        migrationBuilder.DeleteData(
            table: "Roles",
            keyColumn: "Id",
            keyValue: 2);
    }
}
