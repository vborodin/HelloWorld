using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationService.Migrations;

public partial class AddRoleUserRelationship : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "RoleEntityUserEntity",
            columns: table => new
            {
                RolesId = table.Column<long>(type: "bigint", nullable: false),
                UsersId = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RoleEntityUserEntity", x => new { x.RolesId, x.UsersId });
                table.ForeignKey(
                    name: "FK_RoleEntityUserEntity_Roles_RolesId",
                    column: x => x.RolesId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_RoleEntityUserEntity_Users_UsersId",
                    column: x => x.UsersId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_RoleEntityUserEntity_UsersId",
            table: "RoleEntityUserEntity",
            column: "UsersId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "RoleEntityUserEntity");
    }
}
