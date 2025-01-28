using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoCert.Entity.Migrations
{
    /// <inheritdoc />
    public partial class certidcouldbenull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendas_MailAccounts_MailAccountId",
                table: "Agendas");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_Donors_DonorId",
                table: "Certificates");

            migrationBuilder.DropForeignKey(
                name: "FK_Donates_BankAccounts_BankAccountId",
                table: "Donates");

            migrationBuilder.DropForeignKey(
                name: "FK_Donates_Donors_DonorId",
                table: "Donates");

            migrationBuilder.AlterColumn<int>(
                name: "CertificateId",
                table: "Donors",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendas_MailAccounts_MailAccountId",
                table: "Agendas",
                column: "MailAccountId",
                principalTable: "MailAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_Donors_DonorId",
                table: "Certificates",
                column: "DonorId",
                principalTable: "Donors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_Agendas_MailAccounts_MailAccountId",
                table: "Agendas");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_Donors_DonorId",
                table: "Certificates");

            migrationBuilder.DropForeignKey(
                name: "FK_Donates_BankAccounts_BankAccountId",
                table: "Donates");

            migrationBuilder.DropForeignKey(
                name: "FK_Donates_Donors_DonorId",
                table: "Donates");

            migrationBuilder.AlterColumn<int>(
                name: "CertificateId",
                table: "Donors",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Agendas_MailAccounts_MailAccountId",
                table: "Agendas",
                column: "MailAccountId",
                principalTable: "MailAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_Donors_DonorId",
                table: "Certificates",
                column: "DonorId",
                principalTable: "Donors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
