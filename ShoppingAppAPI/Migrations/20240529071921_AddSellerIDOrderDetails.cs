using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingAppAPI.Migrations
{
    public partial class AddSellerIDOrderDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SellerID",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_SellerID",
                table: "OrderDetails",
                column: "SellerID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Sellers_SellerID",
                table: "OrderDetails",
                column: "SellerID",
                principalTable: "Sellers",
                principalColumn: "SellerID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Sellers_SellerID",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_SellerID",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "SellerID",
                table: "OrderDetails");
        }
    }
}
