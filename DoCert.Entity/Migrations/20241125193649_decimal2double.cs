using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoCert.Entity.Migrations
{
    /// <inheritdoc />
    public partial class decimal2double : Migration
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

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "Donates",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donates_BankAccounts_BankAccountId",
                table: "Donates");

            migrationBuilder.DropForeignKey(
                name: "FK_Donates_Donors_DonorId",
                table: "Donates");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Donates",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");

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
    }
}
