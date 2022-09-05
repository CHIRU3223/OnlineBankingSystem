using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineBankingSystem.Migrations
{
    public partial class AddingRequestsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReqAccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestMsg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Progress = table.Column<bool>(type: "bit", nullable: false),
                    RequestRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Requests_Users_Username",
                        column: x => x.Username,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_Username",
                table: "Requests",
                column: "Username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requests");
        }
    }
}
