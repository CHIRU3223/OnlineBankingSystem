﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnlineBankingSystem.Data;

namespace OnlineBankingSystem.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220822111958_AddingTransactionTable")]
    partial class AddingTransactionTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("OnlineBankingSystem.Models.Account", b =>
                {
                    b.Property<string>("AccountNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("Balance")
                        .HasColumnType("bigint");

                    b.Property<bool>("Checkbook")
                        .HasColumnType("bit");

                    b.Property<bool>("Freezed")
                        .HasColumnType("bit");

                    b.Property<int>("NumberOfTransactions")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("AccountNumber");

                    b.HasIndex("Username");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("OnlineBankingSystem.Models.Transaction", b =>
                {
                    b.Property<long>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FromAccountNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToAccountNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("TransactionAmount")
                        .HasColumnType("bigint");

                    b.Property<string>("TransactionMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TransactionStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TransactionTime")
                        .HasColumnType("datetime2");

                    b.HasKey("TransactionId");

                    b.HasIndex("AccountNumber");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("OnlineBankingSystem.Models.User", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DoB")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NoOfAccounts")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PhoneNo")
                        .HasColumnType("int");

                    b.Property<int>("SSN")
                        .HasColumnType("int");

                    b.Property<DateTime>("UserCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isAdmin")
                        .HasColumnType("bit");

                    b.HasKey("Username");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("OnlineBankingSystem.Models.Account", b =>
                {
                    b.HasOne("OnlineBankingSystem.Models.User", "AccUsername")
                        .WithMany()
                        .HasForeignKey("Username");

                    b.Navigation("AccUsername");
                });

            modelBuilder.Entity("OnlineBankingSystem.Models.Transaction", b =>
                {
                    b.HasOne("OnlineBankingSystem.Models.Account", "ToAccount")
                        .WithMany()
                        .HasForeignKey("AccountNumber");

                    b.Navigation("ToAccount");
                });
#pragma warning restore 612, 618
        }
    }
}
