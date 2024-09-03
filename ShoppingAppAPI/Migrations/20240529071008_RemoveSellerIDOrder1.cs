using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingAppAPI.Migrations
{
    public partial class RemoveSellerIDOrder1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Sellers_SellerID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_SellerID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SellerID",
                table: "Orders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SellerID",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SellerID",
                table: "Orders",
                column: "SellerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Sellers_SellerID",
                table: "Orders",
                column: "SellerID",
                principalTable: "Sellers",
                principalColumn: "SellerID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
