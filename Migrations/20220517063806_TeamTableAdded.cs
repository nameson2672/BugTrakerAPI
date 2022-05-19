using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTrakerAPI.Migrations
{
    public partial class TeamTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    teamId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    teamName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    coverImageLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    teamAvatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createrId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.teamId);
                    table.ForeignKey(
                        name: "FK_Team_AspNetUsers_createrId",
                        column: x => x.createrId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Team_createrId",
                table: "Team",
                column: "createrId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Team");
        }
    }
}
