using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoCert.Entity.Migrations
{
    /// <inheritdoc />
    public partial class seconf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donates_BankAccounts_BankAccountId",
                table: "Donates");

            migrationBuilder.DropForeignKey(
                name: "FK_Donates_Donors_DonorId",
                table: "Donates");

            migrationBuilder.DeleteData(
                table: "Donates",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BankAccounts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Donors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "ConstantSymbol",
                table: "Donates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Donates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SpecificSymbol",
                table: "Donates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VariableSymbol",
                table: "Donates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "__DataSeed",
                columns: table => new
                {
                    ProfileName = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    Version = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSeed", x => x.ProfileName);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Donates_BankAccounts_BankAccountId",
                table: "Donates",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Donates_Donors_DonorId",
                table: "Donates",
                column: "DonorId",
                principalTable: "Donors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donates_BankAccounts_BankAccountId",
                table: "Donates");

            migrationBuilder.DropForeignKey(
                name: "FK_Donates_Donors_DonorId",
                table: "Donates");

            migrationBuilder.DropTable(
                name: "__DataSeed");

            migrationBuilder.DropColumn(
                name: "ConstantSymbol",
                table: "Donates");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "Donates");

            migrationBuilder.DropColumn(
                name: "SpecificSymbol",
                table: "Donates");

            migrationBuilder.DropColumn(
                name: "VariableSymbol",
                table: "Donates");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Donates_BankAccounts_BankAccountId",
                table: "Donates",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Donates_Donors_DonorId",
                table: "Donates",
                column: "DonorId",
                principalTable: "Donors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
