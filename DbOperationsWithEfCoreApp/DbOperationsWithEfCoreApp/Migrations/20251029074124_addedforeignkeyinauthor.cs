using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbOperationsWithEfCoreApp.Migrations
{
    /// <inheritdoc />
    public partial class addedforeignkeyinauthor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "Authors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authors_CurrencyId",
                table: "Authors",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Currencies_CurrencyId",
                table: "Authors",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Currencies_CurrencyId",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_CurrencyId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Authors");
        }
    }
}
