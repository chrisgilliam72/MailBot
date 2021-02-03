using Microsoft.EntityFrameworkCore.Migrations;

namespace MailScan.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlockedAddresses",
                columns: table => new
                {
                    pkID = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Emai = table.Column<string>(type: "STRING", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockedAddresses", x => x.pkID);
                });

            migrationBuilder.CreateTable(
                name: "BlockedDomains",
                columns: table => new
                {
                    pkID = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Domain = table.Column<string>(type: "STRING", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockedDomains", x => x.pkID);
                });

            migrationBuilder.CreateTable(
                name: "BodyKeywords",
                columns: table => new
                {
                    pkID = table.Column<long>(type: "INT", nullable: false),
                    KeyWord = table.Column<string>(type: "STRING", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyKeywords", x => x.pkID);
                });

            migrationBuilder.CreateTable(
                name: "MailDetails",
                columns: table => new
                {
                    pkID = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MailID = table.Column<string>(type: "STRING", nullable: false),
                    Date = table.Column<string>(type: "DATETIME", nullable: false),
                    Title = table.Column<string>(type: "STRING", nullable: false),
                    From = table.Column<string>(type: "STRING", nullable: false),
                    Rank = table.Column<long>(type: "INT", nullable: false),
                    Body = table.Column<string>(type: "STRING", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailDetails", x => x.pkID);
                });

            migrationBuilder.CreateTable(
                name: "SubjectKeywords",
                columns: table => new
                {
                    pkID = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KeyWord = table.Column<string>(type: "STRING", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectKeywords", x => x.pkID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlockedAddresses");

            migrationBuilder.DropTable(
                name: "BlockedDomains");

            migrationBuilder.DropTable(
                name: "BodyKeywords");

            migrationBuilder.DropTable(
                name: "MailDetails");

            migrationBuilder.DropTable(
                name: "SubjectKeywords");
        }
    }
}
