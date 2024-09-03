using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingAppAPI.Migrations
{
    public partial class Size_CartItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "CartItems");
        }
    }
}
