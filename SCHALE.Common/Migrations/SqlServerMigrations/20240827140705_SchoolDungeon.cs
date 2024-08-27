using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SCHALE.Common.Migrations.SqlServerMigrations
{
    /// <inheritdoc />
    public partial class SchoolDungeon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SchoolDungeonStageHistories",
                columns: table => new
                {
                    ServerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountServerId = table.Column<long>(type: "bigint", nullable: false),
                    StageUniqueId = table.Column<long>(type: "bigint", nullable: false),
                    BestStarRecord = table.Column<long>(type: "bigint", nullable: false),
                    Star1Flag = table.Column<bool>(type: "bit", nullable: false),
                    Star2Flag = table.Column<bool>(type: "bit", nullable: false),
                    Star3Flag = table.Column<bool>(type: "bit", nullable: false),
                    IsClearedEver = table.Column<bool>(type: "bit", nullable: false),
                    StarFlags = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolDungeonStageHistories", x => x.ServerId);
                    table.ForeignKey(
                        name: "FK_SchoolDungeonStageHistories_Accounts_AccountServerId",
                        column: x => x.AccountServerId,
                        principalTable: "Accounts",
                        principalColumn: "ServerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolDungeonStageHistories_AccountServerId",
                table: "SchoolDungeonStageHistories",
                column: "AccountServerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SchoolDungeonStageHistories");
        }
    }
}
