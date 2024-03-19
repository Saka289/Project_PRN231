using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Services.PaymentAPI.Migrations
{
    public partial class Initial_PaymentDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    orderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    isPayed = table.Column<bool>(type: "bit", nullable: false),
                    paymentStatus = table.Column<int>(type: "int", nullable: false),
                    refund = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}
