using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetDownloader.Entity.Migrations
{
    /// <inheritdoc />
    public partial class Creation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountItems",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountName = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "BLOB", nullable: false),
                    Salt = table.Column<byte[]>(type: "BLOB", nullable: false),
                    ApiKey = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountItems", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "TagItems",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TagName = table.Column<string>(type: "TEXT", nullable: false),
                    TagPath = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagItems", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "HostItems",
                columns: table => new
                {
                    HostId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Hostname = table.Column<string>(type: "TEXT", nullable: false),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HostItems", x => x.HostId);
                    table.ForeignKey(
                        name: "FK_HostItems_AccountItems_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AccountItems",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LinkItems",
                columns: table => new
                {
                    LinksId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    HostId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkItems", x => x.LinksId);
                    table.ForeignKey(
                        name: "FK_LinkItems_HostItems_HostId",
                        column: x => x.HostId,
                        principalTable: "HostItems",
                        principalColumn: "HostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinkItems_TagItems_TagId",
                        column: x => x.TagId,
                        principalTable: "TagItems",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HostItems_AccountId",
                table: "HostItems",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkItems_HostId",
                table: "LinkItems",
                column: "HostId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkItems_TagId",
                table: "LinkItems",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinkItems");

            migrationBuilder.DropTable(
                name: "HostItems");

            migrationBuilder.DropTable(
                name: "TagItems");

            migrationBuilder.DropTable(
                name: "AccountItems");
        }
    }
}
