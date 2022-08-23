using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineBankingSystem.Migrations
{
    public partial class AddingTransactionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Users",
            //    columns: table => new
            //    {
            //        Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        PhoneNo = table.Column<int>(type: "int", nullable: false),
            //        SSN = table.Column<int>(type: "int", nullable: false),
            //        DoB = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        UserCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        isAdmin = table.Column<bool>(type: "bit", nullable: false),
            //        email = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        NoOfAccounts = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Users", x => x.Username);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Accounts",
            //    columns: table => new
            //    {
            //        AccountNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        Username = table.Column<string>(type: "nvarchar(450)", nullable: true),
            //        Freezed = table.Column<bool>(type: "bit", nullable: false),
            //        Balance = table.Column<long>(type: "bigint", nullable: false),
            //        NumberOfTransactions = table.Column<int>(type: "int", nullable: false),
            //        Checkbook = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Accounts", x => x.AccountNumber);
            //        table.ForeignKey(
            //            name: "FK_Accounts_Users_Username",
            //            column: x => x.Username,
            //            principalTable: "Users",
            //            principalColumn: "Username",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    TransactionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromAccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToAccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TransactionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionAmount = table.Column<long>(type: "bigint", nullable: false),
                    TransactionStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transaction_Accounts_AccountNumber",
                        column: x => x.AccountNumber,
                        principalTable: "Accounts",
                        principalColumn: "AccountNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Accounts_Username",
            //    table: "Accounts",
            //    column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AccountNumber",
                table: "Transaction",
                column: "AccountNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaction");

            //migrationBuilder.DropTable(
            //    name: "Accounts");

            //migrationBuilder.DropTable(
            //    name: "Users");
        }
    }
}
