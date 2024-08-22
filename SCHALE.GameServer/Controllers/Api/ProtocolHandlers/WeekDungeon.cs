using SCHALE.Common.Database;
using SCHALE.Common.FlatData;
using SCHALE.Common.NetworkProtocol;
using SCHALE.GameServer.Managers;
using SCHALE.GameServer.Services;

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

            return new WeekDungeonEnterBattleResponse()
            {

            };
        }

        [ProtocolHandler(Protocol.WeekDungeon_BattleResult)]
        public ResponsePacket BattleResultHandler(WeekDungeonBattleResultRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey);
            var db = new WeekDungeonStageHistoryDB();
            if(!req.Summary.IsAbort)
            {
                db.IsCleardEver = false;
                if (account.WeekDungeonStageHistories.Any(x => x.StageUniqueId == req.StageUniqueId))
                {
                    db = account.WeekDungeonStageHistories.First();
                    db.IsCleardEver = true;
                }
                else
                {
                    account.WeekDungeonStageHistories.Add(db);
                }
                db.AccountServerId = req.AccountId;
                db.StageUniqueId = req.StageUniqueId;
                // ToDo: Calculate scores
                db.StarGoalRecord = new() { { StarGoalType.Clear, 1 }, { StarGoalType.GetBoxes, 1 }, { StarGoalType.ClearTimeInSec, 1 }, { StarGoalType.AllAlive, 1 } };
                context.SaveChanges();
            } else if(account.WeekDungeonStageHistories.Any(x => x.StageUniqueId == req.StageUniqueId))
            {
                db = account.WeekDungeonStageHistories.First();
            }

            var weekDungeonData = excelTableService.GetTable<WeekDungeonExcelTable>().UnPack().DataList.Where(x => x.StageId == req.StageUniqueId).ToList().First();
            CurrencyUtils.ConsumeCurrencies(ref account, weekDungeonData.StageEnterCostId, weekDungeonData.StageEnterCostAmount);
            context.SaveChanges();

            return new WeekDungeonBattleResultResponse()
            {
                WeekDungeonStageHistoryDB = db,
                LevelUpCharacterDBs = new(),
                ParcelResultDB = ParcelService.GetParcelResult(account),
            };
        }
    }
}
