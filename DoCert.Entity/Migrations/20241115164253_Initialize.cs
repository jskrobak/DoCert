using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoCert.Entity.Migrations
{
    /// <inheritdoc />
    public partial class Initialize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountNumber = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Donors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Birthdate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Donates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    BankAccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    DonorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Donates_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Donates_Donors_DonorId",
                        column: x => x.DonorId,
                        principalTable: "Donors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BankAccounts",
                columns: new[] { "Id", "AccountNumber" },
                values: new object[] { 1, "1234567890/2010" });

            migrationBuilder.InsertData(
                table: "Donors",
                columns: new[] { "Id", "Birthdate", "Email", "Name" },
                values: new object[] { 1, new DateTime(1977, 10, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "jakub.skrobak@gmail.com", "Jakub Škrobák" });

            migrationBuilder.InsertData(
                table: "Donates",
                columns: new[] { "Id", "Amount", "BankAccountId", "Date", "DonorId" },
                values: new object[] { 1, 1000m, 1, new DateTime(2024, 11, 15, 17, 42, 52, 980, DateTimeKind.Local).AddTicks(4342), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Donates_BankAccountId",
                table: "Donates",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Donates_DonorId",
                table: "Donates",
                column: "DonorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Donates");

            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "Donors");
        }
    }
}
