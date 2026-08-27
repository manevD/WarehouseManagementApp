using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddSupplierToStockMovement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "StockMovements",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_SupplierId",
                table: "StockMovements",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovements_Suppliers_SupplierId",
                table: "StockMovements",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockMovements_Suppliers_SupplierId",
                table: "StockMovements");

            migrationBuilder.DropIndex(
                name: "IX_StockMovements_SupplierId",
                table: "StockMovements");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "StockMovements");
        }
    }
}
