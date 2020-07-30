using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tms.Infra.Data.Migrations
{
    public partial class Teste001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ChangeDate = table.Column<DateTime>(nullable: true),
                    DeleteDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    FinishDate = table.Column<DateTime>(nullable: true),
                    TaskState = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubTasks",
                columns: table => new
                {
                    ParentTaskId = table.Column<int>(nullable: false),
                    TaskId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubTasks", x => new { x.ParentTaskId, x.TaskId });
                    table.ForeignKey(
                        name: "FK_SubTasks_Task_ParentTaskId",
                        column: x => x.ParentTaskId,
                        principalTable: "Task",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubTasks_Task_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubTasks_TaskId",
                table: "SubTasks",
                column: "TaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubTasks");

            migrationBuilder.DropTable(
                name: "Task");
        }
    }
}
