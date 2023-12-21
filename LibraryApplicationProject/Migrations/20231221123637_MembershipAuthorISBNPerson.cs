using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApplicationProject.Migrations
{
    /// <inheritdoc />
    public partial class MembershipAuthorISBNPerson : Migration
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
                defaultValue: new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistryDate",
                schema: "library",
                table: "Memberships",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                schema: "library",
                table: "Loans",
                type: "date",
                nullable: true,
                defaultValue: new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                schema: "library",
                table: "Loans",
                type: "date",
                nullable: true,
                defaultValue: new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                schema: "library",
                table: "ISBNs",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Local));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                schema: "library",
                table: "Persons",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistryDate",
                schema: "library",
                table: "Memberships",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                schema: "library",
                table: "Loans",
                type: "date",
                nullable: true,
                defaultValue: new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                schema: "library",
                table: "Loans",
                type: "date",
                nullable: true,
                defaultValue: new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                schema: "library",
                table: "ISBNs",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
