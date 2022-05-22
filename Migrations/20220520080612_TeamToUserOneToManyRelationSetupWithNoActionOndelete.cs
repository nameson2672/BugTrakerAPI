using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTrakerAPI.Migrations
{
    public partial class TeamToUserOneToManyRelationSetupWithNoActionOndelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "createrId",
                table: "Team",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Team_createrId",
                table: "Team",
                column: "createrId");

            migrationBuilder.AddForeignKey(
                name: "FK_Team_AspNetUsers_createrId",
                table: "Team",
                column: "createrId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Team_AspNetUsers_createrId",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Team_createrId",
                table: "Team");

            migrationBuilder.AlterColumn<string>(
                name: "createrId",
                table: "Team",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
