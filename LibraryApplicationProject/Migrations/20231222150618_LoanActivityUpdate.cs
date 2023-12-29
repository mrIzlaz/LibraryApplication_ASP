using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApplicationProject.Migrations
{
    /// <inheritdoc />
    public partial class LoanActivityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "library",
                table: "Loans",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "library",
                table: "Loans");
        }
    }
}
