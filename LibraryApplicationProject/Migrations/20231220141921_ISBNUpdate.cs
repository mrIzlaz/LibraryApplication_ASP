using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApplicationProject.Migrations
{
    /// <inheritdoc />
    public partial class ISBNUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Isbn",
                schema: "library",
                table: "ISBNs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ISBNs_Isbn",
                schema: "library",
                table: "ISBNs",
                column: "Isbn",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ISBNs_Isbn",
                schema: "library",
                table: "ISBNs");

            migrationBuilder.DropColumn(
                name: "Isbn",
                schema: "library",
                table: "ISBNs");
        }
    }
}
