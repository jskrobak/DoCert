﻿// <auto-generated />
using System;
using DoCert.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DoCert.Entity.Migrations
{
    [DbContext(typeof(DoCertDbContext))]
    [Migration("20241216161952_certidcouldbenull")]
    partial class certidcouldbenull
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.11");

            modelBuilder.Entity("DoCert.Model.Agenda", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("BodyTemplate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FooterText")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("IssuerName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("IssuerPosition")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("LogoPng")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<int>("MailAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MailBody")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MailSubject")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Organization")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PlaceAndDateTemplate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("StamperPng")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MailAccountId");

                    b.ToTable("Agendas");
                });

            modelBuilder.Entity("DoCert.Model.BankAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("BankAccounts");
                });

            modelBuilder.Entity("DoCert.Model.Certificate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Amount")
                        .HasColumnType("REAL");

                    b.Property<int>("DonorId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastSentDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DonorId")
                        .IsUnique();

                    b.ToTable("Certificates");
                });

            modelBuilder.Entity("DoCert.Model.Donate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Amount")
                        .HasColumnType("REAL");

                    b.Property<int>("BankAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConstantSymbol")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("DonorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SpecificSymbol")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("VariableSymbol")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BankAccountId");

                    b.HasIndex("DonorId");

                    b.ToTable("Donates");
                });

            modelBuilder.Entity("DoCert.Model.Donor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Birthdate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CertificateId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Donors");
                });

            modelBuilder.Entity("DoCert.Model.ImportProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AccountNumberColumnIndex")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AmountColumnIndex")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ConstantSymbolColumnIndex")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CultureInfo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("DateColumnIndex")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Delimiter")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("DonorNameColumnIndex")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Encoding")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("HasHeaderRecord")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MessageColumnIndex")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("SpecificSymbolColumnIndex")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("VariableSymbolColumnIndex")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ImportProfiles");
                });

            modelBuilder.Entity("DoCert.Model.MailAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bcc")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Host")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Port")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SenderEmail")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("UseSsl")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MailAccounts");
                });

            modelBuilder.Entity("Havit.Data.EntityFrameworkCore.Model.DataSeedVersion", b =>
                {
                    b.Property<string>("ProfileName")
                        .HasMaxLength(250)
                        .HasColumnType("TEXT");

                    b.Property<string>("Version")
                        .HasColumnType("TEXT");

                    b.HasKey("ProfileName")
                        .HasName("PK_DataSeed");

                    b.ToTable("__DataSeed", (string)null);
                });

            modelBuilder.Entity("DoCert.Model.Agenda", b =>
                {
                    b.HasOne("DoCert.Model.MailAccount", "MailAccount")
                        .WithMany()
                        .HasForeignKey("MailAccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("MailAccount");
                });

            modelBuilder.Entity("DoCert.Model.Certificate", b =>
                {
                    b.HasOne("DoCert.Model.Donor", "Donor")
                        .WithOne("Certificate")
                        .HasForeignKey("DoCert.Model.Certificate", "DonorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Donor");
                });

            modelBuilder.Entity("DoCert.Model.Donate", b =>
                {
                    b.HasOne("DoCert.Model.BankAccount", "BankAccount")
                        .WithMany()
                        .HasForeignKey("BankAccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DoCert.Model.Donor", "Donor")
                        .WithMany()
                        .HasForeignKey("DonorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("BankAccount");

                    b.Navigation("Donor");
                });

            modelBuilder.Entity("DoCert.Model.Donor", b =>
                {
                    b.Navigation("Certificate")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
