using SCHALE.Common.Database;
using SCHALE.Common.Database.ModelExtensions;
using SCHALE.Common.FlatData;
using SCHALE.Common.Migrations.SqlServerMigrations;
using SCHALE.Common.NetworkProtocol;
using SCHALE.GameServer.Managers;
using SCHALE.GameServer.Services;
using Microsoft.EntityFrameworkCore;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class WeekDungeon : ProtocolHandlerBase
    {
        private readonly ISessionKeyService sessionKeyService;
        private readonly SCHALEContext context;
        private readonly ExcelTableService excelTableService;

        public WeekDungeon(IProtocolHandlerFactory protocolHandlerFactory, ISessionKeyService _sessionKeyService, SCHALEContext _context, ExcelTableService _excelTableService) : base(protocolHandlerFactory)
        {
            sessionKeyService = _sessionKeyService;
            context = _context;
            excelTableService = _excelTableService;
        }

        [ProtocolHandler(Protocol.WeekDungeon_List)]
        public ResponsePacket ListHandler(WeekDungeonListRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey);

            return new WeekDungeonListResponse()
            {
                AdditionalStageIdList = new(),
                WeekDungeonStageHistoryDBList = account.WeekDungeonStageHistories.ToList(),
            };
        }

        [ProtocolHandler(Protocol.WeekDungeon_EnterBattle)]
        public ResponsePacket EnterBattleHandler(WeekDungeonEnterBattleRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey);

            var weekDungeonData = excelTableService.GetTable<WeekDungeonExcelTable>().UnPack().DataList.Where(x => x.StageId == req.StageUniqueId).ToList().First();
            CurrencyUtils.ChangeCurrencies(ref account, weekDungeonData.StageEnterCostId, weekDungeonData.StageEnterCostAmount);
            context.Entry(account.Currencies.First()).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();

            return new WeekDungeonEnterBattleResponse()
            {
                ParcelResultDB = new()
                {
                    AccountCurrencyDB = account.Currencies.First(),
                }
            };
        }

        [ProtocolHandler(Protocol.WeekDungeon_BattleResult)]
        public ResponsePacket BattleResultHandler(WeekDungeonBattleResultRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey);
            var currencies = account.Currencies.First();
            var historyDb = new WeekDungeonStageHistoryDB() { AccountServerId = req.AccountId, StageUniqueId = req.StageUniqueId };
            var weekDungeonData = excelTableService.GetTable<WeekDungeonExcelTable>().UnPack().DataList.Where(x => x.StageId == req.StageUniqueId).ToList().First();

            if (!req.Summary.IsAbort && req.Summary.EndType == Common.MX.Logic.Battles.BattleEndType.Clear)
            {
                historyDb.StarGoalRecord = ScoreService.CalculateScore(req.Summary, weekDungeonData.StarGoal, weekDungeonData.StarGoalAmount);

                if (account.WeekDungeonStageHistories.Any(x => x.StageUniqueId == req.StageUniqueId))
                {
                    var existStarGoalRecord = account.WeekDungeonStageHistories.Where(x => x.StageUniqueId == req.StageUniqueId).First().StarGoalRecord;
                    foreach (var goalPair in historyDb.StarGoalRecord)
                    {
                        if (existStarGoalRecord.ContainsKey(goalPair.Key))
                        {
                            if(goalPair.Value > existStarGoalRecord[goalPair.Key])
                            {
                                existStarGoalRecord[goalPair.Key] = goalPair.Value;
                            }
                        }
                        else
                        {
                            existStarGoalRecord.Add(goalPair.Key, goalPair.Value);
                        }
                    }

                    context.Entry(account.WeekDungeonStageHistories.First()).State = EntityState.Modified;
                }
                else
                {
                    account.WeekDungeonStageHistories.Add(historyDb);
                }
            }
            else
            {
                // Return currencies
                CurrencyUtils.ChangeCurrencies(ref account, weekDungeonData.StageEnterCostId, weekDungeonData.StageEnterCostAmount, true);

                context.Entry(currencies).State = EntityState.Modified;
            }

            context.SaveChanges();

            return new WeekDungeonBattleResultResponse()
            {
                WeekDungeonStageHistoryDB = historyDb,
                LevelUpCharacterDBs = new(),
                ParcelResultDB = new()
                {
                    AccountCurrencyDB = currencies,
                },
            };
        }
    }
}
