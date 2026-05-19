using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace boklista_api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuoteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookQuotes");

            migrationBuilder.AddColumn<Guid>(
                name: "BookId",
                table: "Quotes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_BookId",
                table: "Quotes",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_Books_BookId",
                table: "Quotes",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_Books_BookId",
                table: "Quotes");

            migrationBuilder.DropIndex(
                name: "IX_Quotes_BookId",
                table: "Quotes");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Quotes");

            migrationBuilder.CreateTable(
                name: "BookQuotes",
                columns: table => new
                {
                    BooksId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuotesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookQuotes", x => new { x.BooksId, x.QuotesId });
                    table.ForeignKey(
                        name: "FK_BookQuotes_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookQuotes_Quotes_QuotesId",
                        column: x => x.QuotesId,
                        principalTable: "Quotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookQuotes_QuotesId",
                table: "BookQuotes",
                column: "QuotesId");
        }
    }
}
