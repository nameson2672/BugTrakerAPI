using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTrakerAPI.Migrations
{
    public partial class avatarlinkadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarLink",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarLink",
                table: "AspNetUsers");
        }
    }
}
