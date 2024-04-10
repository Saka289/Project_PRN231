using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Services.OrderAPI.Migrations
{
    public partial class OrderDbUpdateProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProducId",
                table: "OrderDetails",
                newName: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "OrderDetails",
                newName: "ProducId");
        }
    }
}
