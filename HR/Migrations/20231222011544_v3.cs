using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.Migrations
{
    public partial class v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "positions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_positions_DepartmentId",
                table: "positions",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_positions_Departments_DepartmentId",
                table: "positions",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_positions_Departments_DepartmentId",
                table: "positions");

            migrationBuilder.DropIndex(
                name: "IX_positions_DepartmentId",
                table: "positions");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "positions");
        }
    }
}
