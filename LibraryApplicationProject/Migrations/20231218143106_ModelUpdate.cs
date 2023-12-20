using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApplicationProject.Migrations
{
    /// <inheritdoc />
    public partial class ModelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorISBN_ISBN_Isbn_Id",
                schema: "library",
                table: "AuthorISBN");

            migrationBuilder.RenameTable(
                name: "ISBN",
                schema: "library",
                newName: "ISBNs",
                newSchema: "library");

            migrationBuilder.CreateTable(
                name: "Memberships",
                schema: "library",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistryDate = table.Column<DateTime>(type: "date", nullable: false, defaultValue: new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Local)),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PersonId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_MembershipId", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Memberships_Persons_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "library",
                        principalTable: "Persons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                schema: "library",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MembershipId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true, defaultValue: new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Local)),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true, defaultValue: new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Local))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_LoanId", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loans_Memberships_MembershipId",
                        column: x => x.MembershipId,
                        principalSchema: "library",
                        principalTable: "Memberships",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rating",
                schema: "library",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReaderRating = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)2),
                    Isbn_Id = table.Column<long>(type: "bigint", nullable: true),
                    MembershipId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_RatingId", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rating_ISBNs_Isbn_Id",
                        column: x => x.Isbn_Id,
                        principalSchema: "library",
                        principalTable: "ISBNs",
                        principalColumn: "Isbn_Id");
                    table.ForeignKey(
                        name: "FK_Rating_Memberships_MembershipId",
                        column: x => x.MembershipId,
                        principalSchema: "library",
                        principalTable: "Memberships",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Books",
                schema: "library",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Isbn_Id = table.Column<long>(type: "bigint", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    LoanId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_BookId", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_ISBNs_Isbn_Id",
                        column: x => x.Isbn_Id,
                        principalSchema: "library",
                        principalTable: "ISBNs",
                        principalColumn: "Isbn_Id");
                    table.ForeignKey(
                        name: "FK_Books_Loans_LoanId",
                        column: x => x.LoanId,
                        principalSchema: "library",
                        principalTable: "Loans",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_Isbn_Id",
                schema: "library",
                table: "Books",
                column: "Isbn_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Books_LoanId",
                schema: "library",
                table: "Books",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_MembershipId",
                schema: "library",
                table: "Loans",
                column: "MembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_PersonId",
                schema: "library",
                table: "Memberships",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_Isbn_Id",
                schema: "library",
                table: "Rating",
                column: "Isbn_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_MembershipId",
                schema: "library",
                table: "Rating",
                column: "MembershipId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorISBN_ISBNs_Isbn_Id",
                schema: "library",
                table: "AuthorISBN",
                column: "Isbn_Id",
                principalSchema: "library",
                principalTable: "ISBNs",
                principalColumn: "Isbn_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorISBN_ISBNs_Isbn_Id",
                schema: "library",
                table: "AuthorISBN");

            migrationBuilder.DropTable(
                name: "Books",
                schema: "library");

            migrationBuilder.DropTable(
                name: "Rating",
                schema: "library");

            migrationBuilder.DropTable(
                name: "Loans",
                schema: "library");

            migrationBuilder.DropTable(
                name: "Memberships",
                schema: "library");

            migrationBuilder.RenameTable(
                name: "ISBNs",
                schema: "library",
                newName: "ISBN",
                newSchema: "library");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorISBN_ISBN_Isbn_Id",
                schema: "library",
                table: "AuthorISBN",
                column: "Isbn_Id",
                principalSchema: "library",
                principalTable: "ISBN",
                principalColumn: "Isbn_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
