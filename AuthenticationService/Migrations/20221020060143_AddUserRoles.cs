using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AuthenticationService.Migrations;

public partial class AddUserRoles : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<long>(
            name: "Id",
            table: "Roles",
            type: "bigint",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "integer")
            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
            .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

        migrationBuilder.CreateTable(
            name: "RoleUser",
            columns: table => new
            {
                RoleId = table.Column<long>(type: "bigint", nullable: false),
                UserId = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RoleUser", x => new { x.RoleId, x.UserId });
                table.ForeignKey(
                    name: "FK_RoleUser_Roles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_RoleUser_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "RoleUser");

        migrationBuilder.AlterColumn<int>(
            name: "Id",
            table: "Roles",
            type: "integer",
            nullable: false,
            oldClrType: typeof(long),
            oldType: "bigint")
            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
            .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
    }
}
