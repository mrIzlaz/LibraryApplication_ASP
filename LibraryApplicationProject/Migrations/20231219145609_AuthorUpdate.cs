using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApplicationProject.Migrations
{
    /// <inheritdoc />
    public partial class AuthorUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                schema: "library",
                table: "Persons",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistryDate",
                schema: "library",
                table: "Memberships",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                schema: "library",
                table: "Loans",
                type: "date",
                nullable: true,
                defaultValue: new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                schema: "library",
                table: "Loans",
                type: "date",
                nullable: true,
                defaultValue: new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                schema: "library",
                table: "ISBNs",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "library",
                table: "Authors",
                type: "varchar(2000)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "library",
                table: "Authors");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                schema: "library",
                table: "Persons",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistryDate",
                schema: "library",
                table: "Memberships",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                schema: "library",
                table: "Loans",
                type: "date",
                nullable: true,
                defaultValue: new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                schema: "library",
                table: "Loans",
                type: "date",
                nullable: true,
                defaultValue: new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                schema: "library",
                table: "ISBNs",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
