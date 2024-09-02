using SCHALE.Common.Database;
using SCHALE.Common.FlatData;
using SCHALE.Common.Utils;
using SCHALE.GameServer.Services;
using SCHALE.GameServer.Services.Irc;

namespace SCHALE.GameServer.Commands
{
    [CommandHandler("unlockall", "Command to unlock all of its contents (campaign, weekdungeon, schooldungeon)", "/unlockall [target content]")]
    internal class UnlockAllCommand : Command
    {
        public UnlockAllCommand(IrcConnection connection, string[] args, bool validate = true) : base(connection, args, validate) { }

        [Argument(0, @"", "Target content name", ArgumentFlags.IgnoreCase)]
        public string target { get; set; } = string.Empty;

        public override void Execute()
        {
            switch (target)
            {
                case "campaign":
                    var campaignChapterExcel = connection.ExcelTableService.GetTable<CampaignChapterExcelTable>().UnPack().DataList;
                    var account = connection.Account;

                    foreach (var excel in campaignChapterExcel)
                    {
                        foreach (var stageId in excel.NormalCampaignStageId.Concat(excel.HardCampaignStageId).Concat(excel.NormalExtraStageId).Concat(excel.VeryHardCampaignStageId))
                        {
                            account.CampaignStageHistories.Add(new()
                            {
                                AccountServerId = account.ServerId,
                                StageUniqueId = stageId,
                                ChapterUniqueId = excel.Id,
                                ClearTurnRecord = 1,
                                Star1Flag = true,
                                Star2Flag = true,
                                Star3Flag = true,
                                LastPlay = DateTime.Now,
                                TodayPlayCount = 1,
                                FirstClearRewardReceive = DateTime.Now,
                                StarRewardReceive = DateTime.Now,
                            });
                        }
                    }

                    connection.Context.SaveChanges();
                    connection.SendChatMessage("Unlocked all of stages of campaign!");
                    break;
            }
        }
    }
}
