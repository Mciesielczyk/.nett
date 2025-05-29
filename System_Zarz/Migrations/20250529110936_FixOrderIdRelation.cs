using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace System_Zarz.Migrations
{
    /// <inheritdoc />
    public partial class FixOrderIdRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId1",
                table: "OrderTasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderId1",
                table: "OrderParts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TaskParts",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    PartId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskParts", x => new { x.TaskId, x.PartId });
                    table.ForeignKey(
                        name: "FK_TaskParts_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskParts_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderTasks_OrderId1",
                table: "OrderTasks",
                column: "OrderId1");

            migrationBuilder.CreateIndex(
                name: "IX_OrderParts_OrderId1",
                table: "OrderParts",
                column: "OrderId1");

            migrationBuilder.CreateIndex(
                name: "IX_TaskParts_PartId",
                table: "TaskParts",
                column: "PartId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderParts_Orders_OrderId1",
                table: "OrderParts",
                column: "OrderId1",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTasks_Orders_OrderId1",
                table: "OrderTasks",
                column: "OrderId1",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderParts_Orders_OrderId1",
                table: "OrderParts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderTasks_Orders_OrderId1",
                table: "OrderTasks");

            migrationBuilder.DropTable(
                name: "TaskParts");

            migrationBuilder.DropIndex(
                name: "IX_OrderTasks_OrderId1",
                table: "OrderTasks");

            migrationBuilder.DropIndex(
                name: "IX_OrderParts_OrderId1",
                table: "OrderParts");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                table: "OrderTasks");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                table: "OrderParts");
        }
    }
}
