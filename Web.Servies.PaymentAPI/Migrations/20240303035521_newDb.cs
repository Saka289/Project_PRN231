using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Services.PaymentAPI.Migrations
{
    public partial class newDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "paymentStatus",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "refund",
                table: "Payments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "paymentStatus",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "refund",
                table: "Payments");
        }
    }
}
