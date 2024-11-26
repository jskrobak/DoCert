using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoCert.Entity.Migrations
{
    /// <inheritdoc />
    public partial class importprofile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImportProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Encoding = table.Column<string>(type: "TEXT", nullable: false),
                    Delimiter = table.Column<string>(type: "TEXT", nullable: false),
                    CultureInfo = table.Column<string>(type: "TEXT", nullable: false),
                    DonorNameColumnIndex = table.Column<int>(type: "INTEGER", nullable: true),
                    AmountColumnIndex = table.Column<int>(type: "INTEGER", nullable: true),
                    DateColumnIndex = table.Column<int>(type: "INTEGER", nullable: true),
                    AccountNumberColumnIndex = table.Column<int>(type: "INTEGER", nullable: true),
                    VariableSymbolColumnIndex = table.Column<int>(type: "INTEGER", nullable: true),
                    SpecificSymbolColumnIndex = table.Column<int>(type: "INTEGER", nullable: true),
                    ConstantSymbolColumnIndex = table.Column<int>(type: "INTEGER", nullable: true),
                    MessageColumnIndex = table.Column<int>(type: "INTEGER", nullable: true),
                    HasHeaderRecord = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportProfiles", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportProfiles");
        }
    }
}
