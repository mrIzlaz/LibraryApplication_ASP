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
                name: "ISBNs",
                schema: "library",
                columns: table => new
                {
                    Isbn_Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Isbn = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "varchar(128)", nullable: false),
                    Description = table.Column<string>(type: "varchar(1200)", nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: false, defaultValue: new DateOnly(2024, 1, 8))
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
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false, defaultValue: new DateOnly(2024, 1, 8))
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
                    Description = table.Column<string>(type: "varchar(2000)", nullable: true),
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
                name: "Memberships",
                schema: "library",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardNumber = table.Column<long>(type: "bigint", nullable: false),
                    RegistryDate = table.Column<DateOnly>(type: "date", nullable: false, defaultValue: new DateOnly(2024, 1, 8)),
                    ExpirationDate = table.Column<DateOnly>(type: "date", nullable: true),
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
                        name: "FK_AuthorISBN_ISBNs_Isbn_Id",
                        column: x => x.Isbn_Id,
                        principalSchema: "library",
                        principalTable: "ISBNs",
                        principalColumn: "Isbn_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                schema: "library",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MembershipId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false, defaultValue: new DateOnly(2024, 1, 8)),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false, defaultValue: new DateOnly(2024, 1, 8)),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
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
                    ReaderRating = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
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
                name: "IX_AuthorISBN_Isbn_Id",
                schema: "library",
                table: "AuthorISBN",
                column: "Isbn_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_PersonId",
                schema: "library",
                table: "Authors",
                column: "PersonId");

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
                name: "IX_ISBNs_Isbn",
                schema: "library",
                table: "ISBNs",
                column: "Isbn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_MembershipId",
                schema: "library",
                table: "Loans",
                column: "MembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_CardNumber",
                schema: "library",
                table: "Memberships",
                column: "CardNumber",
                unique: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorISBN",
                schema: "library");

            migrationBuilder.DropTable(
                name: "Books",
                schema: "library");

            migrationBuilder.DropTable(
                name: "Rating",
                schema: "library");

            migrationBuilder.DropTable(
                name: "Authors",
                schema: "library");

            migrationBuilder.DropTable(
                name: "Loans",
                schema: "library");

            migrationBuilder.DropTable(
                name: "ISBNs",
                schema: "library");

            migrationBuilder.DropTable(
                name: "Memberships",
                schema: "library");

            migrationBuilder.DropTable(
                name: "Persons",
                schema: "library");
        }
    }
}
