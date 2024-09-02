using SCHALE.Common.Database;
using SCHALE.Common.FlatData;
using SCHALE.GameServer.Services;
using SCHALE.Common.MX.Data;

public class CampaignService
{
    public static CampaignStageHistoryDB CreateStageHistoryDB(long accountServerId, CampaignStageInfo stageInfo)
    {
        return new CampaignStageHistoryDB() {
            AccountServerId = accountServerId,
            StageUniqueId = stageInfo.UniqueId,
            ChapterUniqueId = stageInfo.ChapterUniqueId,
            TacticClearCountWithRankSRecord = 0,
            ClearTurnRecord = 1,
            Star1Flag = false,
            Star2Flag = false,
            Star3Flag = false,
            LastPlay = DateTime.Now,
            TodayPlayCount = 1,
            FirstClearRewardReceive = DateTime.Now,
            StarRewardReceive = DateTime.Now,
        };
    }

    // Original Implementation
    // What a shitty codes
    public static void CalcStrategySkipStarGoals(CampaignStageHistoryDB historyDB, BattleSummary summary) {
        for (int i = 0; i < 3; i++)
        {
            var endChecking = false;
            switch (i)
            {
                case 0: // All enemies are defeated
                    foreach (var enemy in summary.Group02Summary.Heroes)
                    {
                        if(enemy.DeadFrame == -1)
                        {
                            endChecking = true;
                        }

                        if(endChecking)
                        {
                            break;
                        }
                    }

                    if (endChecking)
                    {
                        break;
                    }

                    historyDB.Star1Flag = true;
                    break;

                case 1: // All enemies are defeated in 120 seconds
                    foreach (var enemy in summary.Group02Summary.Heroes)
                    {
                        if (enemy.DeadFrame == -1)
                        {
                            endChecking = true;
                        }

                        if (endChecking)
                        {
                            break;
                        }
                    }

                    if (endChecking)
                    {
                        break;
                    }

                    historyDB.Star2Flag = summary.EndFrame <= 120 * 30;
                    break;

                case 2: // No one is defeated
                    foreach (var hero in summary.Group01Summary.Heroes)
                    {
                        if (hero.DeadFrame != -1)
                        {
                            endChecking = true;
                        }

                        if (endChecking)
                        {
                            break;
                        }
                    }

                    if (endChecking)
                    {
                        break;
                    }

                    historyDB.Star3Flag = true;
                    break;
            }
        }
    }
}