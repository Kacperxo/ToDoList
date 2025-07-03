using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Migrations
{
    /// <inheritdoc />
    public partial class CorrectTableNameToTaskModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "TaskModel");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_DueDate",
                table: "TaskModel",
                newName: "IX_TaskModel_DueDate");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskModel",
                table: "TaskModel",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskModel",
                table: "TaskModel");

            migrationBuilder.RenameTable(
                name: "TaskModel",
                newName: "Tasks");

            migrationBuilder.RenameIndex(
                name: "IX_TaskModel_DueDate",
                table: "Tasks",
                newName: "IX_Tasks_DueDate");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");
        }
    }
}
