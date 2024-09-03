using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingAppAPI.Migrations
{
    public partial class AddCascadingToCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_CartID",
                table: "CartItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartID",
                table: "CartItems",
                column: "CartID",
                principalTable: "Carts",
                principalColumn: "CartID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_CartID",
                table: "CartItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartID",
                table: "CartItems",
                column: "CartID",
                principalTable: "Carts",
                principalColumn: "CartID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
