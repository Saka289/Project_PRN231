using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Services.OrderAPI.Migrations
{
    public partial class OrderDbUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderIdString",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderIdString",
                table: "Orders");
        }
    }
}
