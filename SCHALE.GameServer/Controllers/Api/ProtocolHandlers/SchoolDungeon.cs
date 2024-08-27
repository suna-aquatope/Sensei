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
    public class SchoolDungeon : ProtocolHandlerBase
    {
        private readonly ISessionKeyService sessionKeyService;
        private readonly SCHALEContext context;
        private readonly ExcelTableService excelTableService;

        public SchoolDungeon(IProtocolHandlerFactory protocolHandlerFactory, ISessionKeyService _sessionKeyService, SCHALEContext _context, ExcelTableService _excelTableService) : base(protocolHandlerFactory)
        {
            sessionKeyService = _sessionKeyService;
            context = _context;
            excelTableService = _excelTableService;
        }

        [ProtocolHandler(Protocol.SchoolDungeon_List)]
        public ResponsePacket ListHandler(SchoolDungeonListRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey);

            return new SchoolDungeonListResponse()
            {
                SchoolDungeonStageHistoryDBList = account.SchoolDungeonStageHistories.ToList(),
            };
        }

        [ProtocolHandler(Protocol.SchoolDungeon_EnterBattle)]
        public ResponsePacket EnterBattleHandler(SchoolDungeonEnterBattleRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey);

            var schoolDungeonData = excelTableService.GetTable<SchoolDungeonStageExcelTable>().UnPack().DataList.Where(x => x.StageId == req.StageUniqueId).ToList().First();
            CurrencyUtils.ChangeCurrencies(ref account, schoolDungeonData.StageEnterCostId, Array.ConvertAll(schoolDungeonData.StageEnterCostAmount.ToArray(), Convert.ToInt32).ToList());
            context.Entry(account.Currencies.First()).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();

            return new SchoolDungeonEnterBattleResponse()
            {
                ParcelResultDB = new()
                {
                    AccountCurrencyDB = account.Currencies.First(),
                }
            };
        }

        [ProtocolHandler(Protocol.SchoolDungeon_BattleResult)]
        public ResponsePacket BattleResultHandler(SchoolDungeonBattleResultRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey);
            var currencies = account.Currencies.First();
            var historyDb = new SchoolDungeonStageHistoryDB() { AccountServerId = req.AccountId, StageUniqueId = req.StageUniqueId };
            var schoolDungeonData = excelTableService.GetTable<SchoolDungeonStageExcelTable>().UnPack().DataList.Where(x => x.StageId == req.StageUniqueId).ToList().First();

            if (!req.Summary.IsAbort && req.Summary.EndType == Common.MX.Logic.Battles.BattleEndType.Clear)
            {
                historyDb.StarFlags = ScoreService.CalculateScoreSchoolDungeon(req.Summary, schoolDungeonData.StarGoal, schoolDungeonData.StarGoalAmount).ToArray();

                if (account.SchoolDungeonStageHistories.Any(x => x.StageUniqueId == req.StageUniqueId))
                {
                    var existStageHistory = account.SchoolDungeonStageHistories.Where(x => x.StageUniqueId == req.StageUniqueId).First();
                    for(var i = 0; i < existStageHistory.StarFlags.Length; i++)
                    {
                        existStageHistory.StarFlags[i] = existStageHistory.StarFlags[i] ? true : historyDb.StarFlags[i];
                    }

                    context.Entry(account.WeekDungeonStageHistories.First()).State = EntityState.Modified;
                }
                else
                {
                    account.SchoolDungeonStageHistories.Add(historyDb);
                }
            }
            else
            {
                // Return currencies
                CurrencyUtils.ChangeCurrencies(ref account, schoolDungeonData.StageEnterCostId, Array.ConvertAll(schoolDungeonData.StageEnterCostAmount.ToArray(), Convert.ToInt32).ToList(), true);

                context.Entry(currencies).State = EntityState.Modified;
            }

            context.SaveChanges();

            return new SchoolDungeonBattleResultResponse()
            {
                SchoolDungeonStageHistoryDB = historyDb,
                LevelUpCharacterDBs = new(),
                ParcelResultDB = new()
                {
                    AccountCurrencyDB = currencies,
                },
            };
        }
    }
}
