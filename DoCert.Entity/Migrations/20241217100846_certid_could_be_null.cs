using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoCert.Entity.Migrations
{
    /// <inheritdoc />
    public partial class certid_could_be_null : Migration
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

            migrationBuilder.DropIndex(
                name: "IX_Certificates_DonorId",
                table: "Certificates");

            migrationBuilder.CreateIndex(
                name: "IX_Donors_CertificateId",
                table: "Donors",
                column: "CertificateId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Agendas_MailAccounts_MailAccountId",
                table: "Agendas",
                column: "MailAccountId",
                principalTable: "MailAccounts",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Donors_Certificates_CertificateId",
                table: "Donors",
                column: "CertificateId",
                principalTable: "Certificates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendas_MailAccounts_MailAccountId",
                table: "Agendas");

            migrationBuilder.DropForeignKey(
                name: "FK_Donates_BankAccounts_BankAccountId",
                table: "Donates");

            migrationBuilder.DropForeignKey(
                name: "FK_Donates_Donors_DonorId",
                table: "Donates");

            migrationBuilder.DropForeignKey(
                name: "FK_Donors_Certificates_CertificateId",
                table: "Donors");

            migrationBuilder.DropIndex(
                name: "IX_Donors_CertificateId",
                table: "Donors");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_DonorId",
                table: "Certificates",
                column: "DonorId",
                unique: true);

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
    }
}
