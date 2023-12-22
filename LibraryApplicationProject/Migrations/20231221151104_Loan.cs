using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApplicationProject.Migrations
{
    /// <inheritdoc />
    public partial class Loan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                schema: "library",
                table: "Loans",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                schema: "library",
                table: "Loans",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Local));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                schema: "library",
                table: "Loans",
                type: "date",
                nullable: true,
                defaultValue: new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                schema: "library",
                table: "Loans",
                type: "date",
                nullable: true,
                defaultValue: new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
