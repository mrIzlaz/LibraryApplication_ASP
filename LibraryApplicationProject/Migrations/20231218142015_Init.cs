using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApplicationProject.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "library");

            migrationBuilder.CreateTable(
                name: "ISBN",
                schema: "library",
                columns: table => new
                {
                    Isbn_Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "varchar(128)", nullable: false),
                    Description = table.Column<string>(type: "varchar(1200)", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "date", nullable: false, defaultValue: new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Local))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_ISBN", x => x.Isbn_Id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                schema: "library",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "varchar(128)", nullable: false),
                    LastName = table.Column<string>(type: "varchar(128)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false, defaultValue: new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Local))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_PersonId", x => x.Id);
                },
                comment: "Register Person for Authors and Clients");

            migrationBuilder.CreateTable(
                name: "Authors",
                schema: "library",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_AuthorId", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authors_Persons_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "library",
                        principalTable: "Persons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AuthorISBN",
                schema: "library",
                columns: table => new
                {
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    Isbn_Id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorISBN", x => new { x.AuthorId, x.Isbn_Id });
                    table.ForeignKey(
                        name: "FK_AuthorISBN_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalSchema: "library",
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorISBN_ISBN_Isbn_Id",
                        column: x => x.Isbn_Id,
                        principalSchema: "library",
                        principalTable: "ISBN",
                        principalColumn: "Isbn_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorISBN_Isbn_Id",
                schema: "library",
                table: "AuthorISBN",
                column: "Isbn_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_PersonId",
                schema: "library",
                table: "Authors",
                column: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorISBN",
                schema: "library");

            migrationBuilder.DropTable(
                name: "Authors",
                schema: "library");

            migrationBuilder.DropTable(
                name: "ISBN",
                schema: "library");

            migrationBuilder.DropTable(
                name: "Persons",
                schema: "library");
        }
    }
}
