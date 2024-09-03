using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingAppAPI.Migrations
{
    public partial class RemoveSellerIDOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Sellers_SellerID",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Sellers_SellerID",
                table: "Orders",
                column: "SellerID",
                principalTable: "Sellers",
                principalColumn: "SellerID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Sellers_SellerID",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Sellers_SellerID",
                table: "Orders",
                column: "SellerID",
                principalTable: "Sellers",
                principalColumn: "SellerID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
