using Microsoft.EntityFrameworkCore.Migrations;

namespace CafeApi.Migrations
{
    public partial class cafeUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Cafes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cafes_CreatedById",
                table: "Cafes",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Cafes_Users_CreatedById",
                table: "Cafes",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cafes_Users_CreatedById",
                table: "Cafes");

            migrationBuilder.DropIndex(
                name: "IX_Cafes_CreatedById",
                table: "Cafes");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Cafes");
        }
    }
}
