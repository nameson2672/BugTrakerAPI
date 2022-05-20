using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTrakerAPI.Migrations
{
    public partial class TeamAdminTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeamAdmins",
                columns: table => new
                {
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    teamId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamAdmins");
        }
    }
}
