using Microsoft.EntityFrameworkCore;
using SCHALE.Common.Database;
using SCHALE.Common.FlatData;
using SCHALE.Common.Migrations.SqlServerMigrations;
using SCHALE.Common.NetworkProtocol;
using SCHALE.Common.Parcel;
using SCHALE.GameServer.Services;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Campaign : ProtocolHandlerBase
    {
        private ISessionKeyService sessionKeyService;
        private SCHALEContext context;
        private ExcelTableService excelTableService;

        public Campaign(IProtocolHandlerFactory protocolHandlerFactory, ISessionKeyService _sessionKeyService, SCHALEContext _context, ExcelTableService _excelTableService) : base(protocolHandlerFactory)
        {
            sessionKeyService = _sessionKeyService;
            context = _context;
            excelTableService = _excelTableService;
        }

        [ProtocolHandler(Protocol.Campaign_List)]
        public ResponsePacket ListHandler(CampaignListRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey);

            return new CampaignListResponse()
            {
                CampaignChapterClearRewardHistoryDBs = new(),
                StageHistoryDBs = account.CampaignStageHistories.ToList(),
                StrategyObjecthistoryDBs = new(),
                DailyResetCountDB = new()
            };
        }

        [ProtocolHandler(Protocol.Campaign_EnterMainStage)]
        public ResponsePacket EnterMainStageHandler(CampaignEnterMainStageRequest req)
        {
            return new CampaignEnterMainStageResponse()
            {
                SaveDataDB = new CampaignMainStageSaveDB()
                {
                    ContentType = ContentType.CampaignMainStage,
                    LastEnemyEntityId = 10010,

                    ActivatedHexaEventsAndConditions = new() { { 0, [0] } },
                    HexaEventDelayedExecutions = [],
                    CreateTime = DateTime.Parse("2024-04-22T18:33:21"),
                    StageUniqueId = 1011101,
                    StageEntranceFee = [],
                    EnemyKillCountByUniqueId = []
                }
            };
        }

        [ProtocolHandler(Protocol.Campaign_EnterSubStage)]
        public ResponsePacket EnterSubStageHandler(CampaignEnterSubStageRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey);
            var campaignExcel = excelTableService.GetTable<CampaignStageExcelTable>().UnPack().DataList.Where(x => x.Id == req.StageUniqueId).ToList().First();

            var costId = campaignExcel.StageEnterCostId;
            var costAmount = campaignExcel.StageEnterCostAmount;
            var currency = account.Currencies.First();
            currency.CurrencyDict[(CurrencyTypes)costId] -= costAmount;
            currency.UpdateTimeDict[(CurrencyTypes)costId] = DateTime.Now;

            context.Entry(currency).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();

            return new CampaignEnterSubStageResponse()
            {
                ParcelResultDB = new()
                {
                    AccountCurrencyDB = currency,
                },
                SaveDataDB = new()
                {
                    ContentType = ContentType.CampaignSubStage,
                }
            };
        }

        [ProtocolHandler(Protocol.Campaign_SubStageResult)]
        public ResponsePacket SubStageResultHandler(CampaignSubStageResultRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey);
            var currencies = account.Currencies.First();
            var campaignChapterExcel = excelTableService.GetTable<CampaignChapterExcelTable>().UnPack().DataList
                .Where(x => x.NormalCampaignStageId.Contains(req.Summary.StageId) || x.HardCampaignStageId.Contains(req.Summary.StageId) || x.NormalExtraStageId.Contains(req.Summary.StageId) || x.VeryHardCampaignStageId.Contains(req.Summary.StageId)).ToList().First();
            var campaignExcel = excelTableService.GetTable<CampaignStageExcelTable>().UnPack().DataList.Where(x => x.Id == req.Summary.StageId).First();
            CampaignStageHistoryDB historyDb = new();
            ParcelResultDB parcelResultDb = new()
            {
                AccountCurrencyDB = currencies,
                DisplaySequence = new()
            };

            if (!req.Summary.IsAbort && req.Summary.EndType == Common.MX.Logic.Battles.BattleEndType.Clear)
            {
                CampaignService.CalcStrategySkipStarGoals(historyDb, req.Summary);

                if (account.CampaignStageHistories.Any(x => x.StageUniqueId == req.Summary.StageId))
                {
                    var existHistory = account.CampaignStageHistories.Where(x => x.StageUniqueId == req.Summary.StageId).First();

                    existHistory.Star1Flag = existHistory.Star1Flag ? true : historyDb.Star1Flag;
                    existHistory.Star2Flag = existHistory.Star2Flag ? true : historyDb.Star2Flag;
                    existHistory.Star3Flag = existHistory.Star3Flag ? true : historyDb.Star3Flag;

                    existHistory.TodayPlayCount += 1;

                    existHistory.LastPlay = DateTime.Now;

                    context.Entry(existHistory).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                    historyDb = existHistory;
                }
                else
                {
                    account.CampaignStageHistories.Add(historyDb);
                }
            }
            else
            {
                // Return currencies
                var costId = campaignExcel.StageEnterCostId;
                var costAmount = campaignExcel.StageEnterCostAmount;
                currencies.CurrencyDict[(CurrencyTypes)costId] += costAmount;
                currencies.UpdateTimeDict[(CurrencyTypes)costId] = DateTime.Now;

                context.Entry(currencies).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                parcelResultDb.DisplaySequence.Add(new()
                {
                    Amount = costAmount,
                    Key = new()
                    {
                        Type = ParcelType.Currency,
                        Id = costId
                    }
                });
            }

            context.SaveChanges();

            return new CampaignSubStageResultResponse()
            {
                TacticRank = 0,
                CampaignStageHistoryDB = historyDb,
                LevelUpCharacterDBs = new(),
                ParcelResultDB = parcelResultDb,
                FirstClearReward = new(),
                ThreeStarReward = new()
            };
        }

        [ProtocolHandler(Protocol.Campaign_EnterTutorialStage)]
        public ResponsePacket EnterTutorialStageHandler(CampaignEnterTutorialStageRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey);
            var campaignExcel = excelTableService.GetTable<CampaignStageExcelTable>().UnPack().DataList.Where(x => x.Id == req.StageUniqueId).ToList().First();

            var costId = campaignExcel.StageEnterCostId;
            var costAmount = campaignExcel.StageEnterCostAmount;
            var currency = account.Currencies.First();
            currency.CurrencyDict[(CurrencyTypes)costId] -= costAmount;
            currency.UpdateTimeDict[(CurrencyTypes)costId] = DateTime.Now;

            context.Entry(currency).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();

            return new CampaignEnterTutorialStageResponse()
            {
                ParcelResultDB = new()
                {
                    AccountCurrencyDB = currency,
                },
                SaveDataDB = new()
                {
                    ContentType = ContentType.CampaignTutorialStage
                }
            };
        }

        [ProtocolHandler(Protocol.Campaign_TutorialStageResult)]
        public ResponsePacket TutorialStageResultHandler(CampaignTutorialStageResultRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey);
            var campaignExcel = excelTableService.GetTable<CampaignChapterExcelTable>().UnPack().DataList
                .Where(x => x.NormalCampaignStageId.Contains(req.Summary.StageId) || x.HardCampaignStageId.Contains(req.Summary.StageId)).ToList().First();
            var historyDb = CampaignService.CreateStageHistoryDB(req.AccountId, new() { UniqueId = req.Summary.StageId, ChapterUniqueId = campaignExcel.Id });
            historyDb.ClearTurnRecord = 1;

            if(!account.CampaignStageHistories.Any(x => x.StageUniqueId == req.Summary.StageId))
            {
                account.CampaignStageHistories.Add(historyDb);
            }

            context.SaveChanges();

            return new CampaignTutorialStageResultResponse()
            {
                CampaignStageHistoryDB = historyDb,
                ParcelResultDB = new(),
                ClearReward = new(),
                FirstClearReward = new(),
            };
        }

        [ProtocolHandler(Protocol.Campaign_EnterMainStageStrategySkip)]
        public ResponsePacket EnterMainStageStrategySkipHandler(CampaignEnterMainStageStrategySkipRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey);
            var campaignExcel = excelTableService.GetTable<CampaignStageExcelTable>().UnPack().DataList.Where(x => x.Id == req.StageUniqueId).ToList().First();

            var costId = campaignExcel.StageEnterCostId;
            var costAmount = campaignExcel.StageEnterCostAmount;
            var currency = account.Currencies.First();
            currency.CurrencyDict[(CurrencyTypes)costId] -= costAmount;
            currency.UpdateTimeDict[(CurrencyTypes)costId] = DateTime.Now;

            context.Entry(currency).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();

            return new CampaignEnterMainStageStrategySkipResponse()
            {
                ParcelResultDB = new()
                {
                    AccountCurrencyDB = currency,
                }
            };
        }

        [ProtocolHandler(Protocol.Campaign_MainStageStrategySkipResult)]
        public ResponsePacket MainStageStrategySkipResultHandler(CampaignMainStageStrategySkipResultRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey);
            var currencies = account.Currencies.First();
            var campaignChapterExcel = excelTableService.GetTable<CampaignChapterExcelTable>().UnPack().DataList
                .Where(x => x.NormalCampaignStageId.Contains(req.Summary.StageId) || x.HardCampaignStageId.Contains(req.Summary.StageId) || x.NormalExtraStageId.Contains(req.Summary.StageId) || x.VeryHardCampaignStageId.Contains(req.Summary.StageId)).ToList().First();
            var campaignExcel = excelTableService.GetTable<CampaignStageExcelTable>().UnPack().DataList.Where(x => x.Id == req.Summary.StageId).First();
            CampaignStageHistoryDB historyDb = new();
            ParcelResultDB parcelResultDb = new()
            {
                AccountCurrencyDB = currencies,
                DisplaySequence = new()
            };

            if (!req.Summary.IsAbort && req.Summary.EndType == Common.MX.Logic.Battles.BattleEndType.Clear)
            {
                CampaignService.CalcStrategySkipStarGoals(historyDb, req.Summary);

                if (account.CampaignStageHistories.Any(x => x.StageUniqueId == req.Summary.StageId))
                {
                    var existHistory = account.CampaignStageHistories.Where(x => x.StageUniqueId == req.Summary.StageId).First();

                    existHistory.Star1Flag = existHistory.Star1Flag ? true : historyDb.Star1Flag;
                    existHistory.Star2Flag = existHistory.Star2Flag ? true : historyDb.Star2Flag;
                    existHistory.Star3Flag = existHistory.Star3Flag ? true : historyDb.Star3Flag;

                    existHistory.TodayPlayCount += 1;

                    existHistory.LastPlay = DateTime.Now;

                    context.Entry(existHistory).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                    historyDb = existHistory;
                }
                else
                {
                    account.CampaignStageHistories.Add(historyDb);
                }
            } else
            {
                // Return currencies
                var costId = campaignExcel.StageEnterCostId;
                var costAmount = campaignExcel.StageEnterCostAmount;
                currencies.CurrencyDict[(CurrencyTypes)costId] += costAmount;
                currencies.UpdateTimeDict[(CurrencyTypes)costId] = DateTime.Now;

                context.Entry(currencies).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                parcelResultDb.DisplaySequence.Add(new()
                {
                    Amount = costAmount,
                    Key = new()
                    {
                        Type = ParcelType.Currency,
                        Id = costId
                    }
                });
            }

            context.SaveChanges();

            return new CampaignMainStageStrategySkipResultResponse()
            {
                TacticRank = 0,
                CampaignStageHistoryDB = historyDb,
                LevelUpCharacterDBs = new(),
                ParcelResultDB = parcelResultDb,
                FirstClearReward = new(),
                ThreeStarReward = new()
            };
        }
    }
}
