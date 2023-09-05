using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeesManager.Migrations
{
    public partial class FixForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Supervisor",
                table: "Employees",
                newName: "SupervisorID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SupervisorID",
                table: "Employees",
                column: "SupervisorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_SupervisorID",
                table: "Employees",
                column: "SupervisorID",
                principalTable: "Employees",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_SupervisorID",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_SupervisorID",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "SupervisorID",
                table: "Employees",
                newName: "Supervisor");
        }
    }
}
