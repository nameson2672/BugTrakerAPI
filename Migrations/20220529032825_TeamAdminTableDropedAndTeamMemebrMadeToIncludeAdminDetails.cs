using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTrakerAPI.Migrations
{
    public partial class TeamAdminTableDropedAndTeamMemebrMadeToIncludeAdminDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamMembers_AspNetUsers_UserId",
                table: "TeamMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamMembers_Team_TeamId",
                table: "TeamMembers");

            migrationBuilder.DropTable(
                name: "TeamAdmins");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "TeamMembers",
                newName: "teamId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TeamMembers",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_TeamMembers_TeamId",
                table: "TeamMembers",
                newName: "IX_TeamMembers_teamId");

            migrationBuilder.AddColumn<bool>(
                name: "isUserAdmin",
                table: "TeamMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_AspNetUsers_userId",
                table: "TeamMembers",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_Team_teamId",
                table: "TeamMembers",
                column: "teamId",
                principalTable: "Team",
                principalColumn: "teamId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamMembers_AspNetUsers_userId",
                table: "TeamMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamMembers_Team_teamId",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "isUserAdmin",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "teamId",
                table: "TeamMembers",
                newName: "TeamId");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "TeamMembers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TeamMembers_teamId",
                table: "TeamMembers",
                newName: "IX_TeamMembers_TeamId");

            migrationBuilder.CreateTable(
                name: "TeamAdmins",
                columns: table => new
                {
                    teamId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamAdmins", x => new { x.teamId, x.userId });
                    table.ForeignKey(
                        name: "FK_TeamAdmins_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamAdmins_Team_teamId",
                        column: x => x.teamId,
                        principalTable: "Team",
                        principalColumn: "teamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamAdmins_userId",
                table: "TeamAdmins",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_AspNetUsers_UserId",
                table: "TeamMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_Team_TeamId",
                table: "TeamMembers",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "teamId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
