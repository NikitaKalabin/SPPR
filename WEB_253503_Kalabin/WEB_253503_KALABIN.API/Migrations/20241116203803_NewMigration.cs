using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_253503_KALABIN.API.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Automobiles_AutomobileCategories_CategoryId",
                table: "Automobiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Automobiles",
                table: "Automobiles");

            migrationBuilder.DropIndex(
                name: "IX_Automobiles_CategoryId",
                table: "Automobiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AutomobileCategories",
                table: "AutomobileCategories");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Automobiles");

            migrationBuilder.RenameTable(
                name: "Automobiles",
                newName: "Clothes");

            migrationBuilder.RenameTable(
                name: "AutomobileCategories",
                newName: "ClothesCategories");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Clothes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clothes",
                table: "Clothes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClothesCategories",
                table: "ClothesCategories",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ClothesCategories",
                table: "ClothesCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clothes",
                table: "Clothes");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Clothes");

            migrationBuilder.RenameTable(
                name: "ClothesCategories",
                newName: "AutomobileCategories");

            migrationBuilder.RenameTable(
                name: "Clothes",
                newName: "Automobiles");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Automobiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AutomobileCategories",
                table: "AutomobileCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Automobiles",
                table: "Automobiles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Automobiles_CategoryId",
                table: "Automobiles",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Automobiles_AutomobileCategories_CategoryId",
                table: "Automobiles",
                column: "CategoryId",
                principalTable: "AutomobileCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
