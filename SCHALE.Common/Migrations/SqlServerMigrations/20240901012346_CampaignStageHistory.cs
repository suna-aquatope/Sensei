using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SCHALE.Common.Migrations.SqlServerMigrations
{
    /// <inheritdoc />
    public partial class CampaignStageHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CampaignStageHistories",
                columns: table => new
                {
                    ServerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountServerId = table.Column<long>(type: "bigint", nullable: false),
                    StoryUniqueId = table.Column<long>(type: "bigint", nullable: false),
                    ChapterUniqueId = table.Column<long>(type: "bigint", nullable: false),
                    StageUniqueId = table.Column<long>(type: "bigint", nullable: false),
                    TacticClearCountWithRankSRecord = table.Column<long>(type: "bigint", nullable: false),
                    ClearTurnRecord = table.Column<long>(type: "bigint", nullable: false),
                    BestStarRecord = table.Column<long>(type: "bigint", nullable: false),
                    Star1Flag = table.Column<bool>(type: "bit", nullable: false),
                    Star2Flag = table.Column<bool>(type: "bit", nullable: false),
                    Star3Flag = table.Column<bool>(type: "bit", nullable: false),
                    LastPlay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TodayPlayCount = table.Column<long>(type: "bigint", nullable: false),
                    TodayPurchasePlayCountHardStage = table.Column<long>(type: "bigint", nullable: false),
                    FirstClearRewardReceive = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StarRewardReceive = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsClearedEver = table.Column<bool>(type: "bit", nullable: false),
                    TodayPlayCountForUI = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignStageHistories", x => x.ServerId);
                    table.ForeignKey(
                        name: "FK_CampaignStageHistories_Accounts_AccountServerId",
                        column: x => x.AccountServerId,
                        principalTable: "Accounts",
                        principalColumn: "ServerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignStageHistories_AccountServerId",
                table: "CampaignStageHistories",
                column: "AccountServerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignStageHistories");
        }
    }
}
