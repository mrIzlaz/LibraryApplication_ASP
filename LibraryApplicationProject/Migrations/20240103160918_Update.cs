using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApplicationProject.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "BirthDate",
                schema: "library",
                table: "Persons",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(2024, 1, 3),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValue: new DateOnly(2024, 1, 2));

            migrationBuilder.AlterColumn<DateOnly>(
                name: "RegistryDate",
                schema: "library",
                table: "Memberships",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(2024, 1, 3),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValue: new DateOnly(2024, 1, 2));

            migrationBuilder.AlterColumn<DateOnly>(
                name: "StartDate",
                schema: "library",
                table: "Loans",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(2024, 1, 3),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValue: new DateOnly(2024, 1, 2));

            migrationBuilder.AlterColumn<DateOnly>(
                name: "EndDate",
                schema: "library",
                table: "Loans",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(2024, 1, 3),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValue: new DateOnly(2024, 1, 2));

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ReleaseDate",
                schema: "library",
                table: "ISBNs",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(2024, 1, 3),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValue: new DateOnly(2024, 1, 2));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "BirthDate",
                schema: "library",
                table: "Persons",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(2024, 1, 2),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValue: new DateOnly(2024, 1, 3));

            migrationBuilder.AlterColumn<DateOnly>(
                name: "RegistryDate",
                schema: "library",
                table: "Memberships",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(2024, 1, 2),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValue: new DateOnly(2024, 1, 3));

            migrationBuilder.AlterColumn<DateOnly>(
                name: "StartDate",
                schema: "library",
                table: "Loans",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(2024, 1, 2),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValue: new DateOnly(2024, 1, 3));

            migrationBuilder.AlterColumn<DateOnly>(
                name: "EndDate",
                schema: "library",
                table: "Loans",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(2024, 1, 2),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValue: new DateOnly(2024, 1, 3));

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ReleaseDate",
                schema: "library",
                table: "ISBNs",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(2024, 1, 2),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValue: new DateOnly(2024, 1, 3));
        }
    }
}
