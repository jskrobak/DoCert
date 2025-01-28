using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoCert.Entity.Migrations
{
    /// <inheritdoc />
    public partial class removeaccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donates_BankAccounts_BankAccountId",
                table: "Donates");

            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "ImportProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Donates_BankAccountId",
                table: "Donates");

            migrationBuilder.AddColumn<string>(
                name: "Iban",
                table: "Donates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Iban",
                table: "Donates");

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
                name: "ImportProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountNumberColumnIndex = table.Column<int>(type: "INTEGER", nullable: true),
                    AmountColumnIndex = table.Column<int>(type: "INTEGER", nullable: true),
                    ConstantSymbolColumnIndex = table.Column<int>(type: "INTEGER", nullable: true),
                    CultureInfo = table.Column<string>(type: "TEXT", nullable: false),
                    DateColumnIndex = table.Column<int>(type: "INTEGER", nullable: true),
                    Delimiter = table.Column<string>(type: "TEXT", nullable: false),
                    DonorNameColumnIndex = table.Column<int>(type: "INTEGER", nullable: true),
                    Encoding = table.Column<string>(type: "TEXT", nullable: false),
                    HasHeaderRecord = table.Column<bool>(type: "INTEGER", nullable: false),
                    MessageColumnIndex = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SpecificSymbolColumnIndex = table.Column<int>(type: "INTEGER", nullable: true),
                    VariableSymbolColumnIndex = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportProfiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Donates_BankAccountId",
                table: "Donates",
                column: "BankAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Donates_BankAccounts_BankAccountId",
                table: "Donates",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
