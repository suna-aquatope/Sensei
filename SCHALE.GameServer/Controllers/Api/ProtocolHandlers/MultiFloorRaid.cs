using SCHALE.Common.Database;
using SCHALE.Common.NetworkProtocol;
using SCHALE.GameServer.Services;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class MultiFloorRaid : ProtocolHandlerBase
    {
        private readonly ISessionKeyService sessionKeyService;
        private readonly SCHALEContext context;
        private readonly ExcelTableService excelTableService;

        public MultiFloorRaid(IProtocolHandlerFactory protocolHandlerFactory, ISessionKeyService _sessionKeyService, SCHALEContext _context, ExcelTableService _excelTableService) : base(protocolHandlerFactory)
        {
            sessionKeyService = _sessionKeyService;
            context = _context;
            excelTableService = _excelTableService;
        }

        [ProtocolHandler(Protocol.MultiFloorRaid_Sync)]
        public ResponsePacket SyncHandler(MultiFloorRaidSyncRequest req)
        {
            var raidList = sessionKeyService.GetAccount(req.SessionKey).MultiFloorRaids.ToList();
            return new MultiFloorRaidSyncResponse()
            {
                MultiFloorRaidDBs = raidList.Count == 0 ? new List<MultiFloorRaidDB>() { new() { SeasonId = (long)req.SeasonId } } : raidList,
            };
        }

        [ProtocolHandler(Protocol.MultiFloorRaid_EnterBattle)]
        public ResponsePacket EnterBattleHandler(MultiFloorRaidEnterBattleRequest req)
        {
            return new MultiFloorRaidEnterBattleResponse()
            {
                AssistCharacterDBs = new()
            };
        }

        [ProtocolHandler(Protocol.MultiFloorRaid_EndBattle)]
        public ResponsePacket EndBattleHandler(MultiFloorRaidEndBattleRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey);
            MultiFloorRaidDB db = new();

            if (!req.Summary.IsAbort)
            {
                if (account.MultiFloorRaids.Any(x => x.ClearedDifficulty == req.Difficulty))
                {
                    db = account.MultiFloorRaids.Where(x => x.ClearedDifficulty == req.Difficulty).First();
                }
                else
                {
                    account.MultiFloorRaids.Add(db);
                }
                db.SeasonId = req.SeasonId;
                db.ClearedDifficulty = req.Difficulty;
                db.LastClearDate = DateTime.Now;
                db.RewardDifficulty = req.Difficulty;
                db.LastRewardDate = DateTime.Now;
                db.ClearBattleFrame = req.Summary.EndFrame;
                db.AllCleared = false;
                db.HasReceivableRewards = false;
                db.TotalReceivedRewards = new();
                db.TotalReceivableRewards = new();
                context.SaveChanges();
            }

            return new MultiFloorRaidEndBattleResponse()
            {
                MultiFloorRaidDB = db,
                ParcelResultDB = ParcelService.GetParcelResult(account)
            };
        }

        [ProtocolHandler(Protocol.MultiFloorRaid_ReceiveReward)]
        public ResponsePacket RecieveRewardHandler(MultiFloorRaidEndBattleRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey);
            return new MultiFloorRaidEndBattleResponse()
            {
                MultiFloorRaidDB = sessionKeyService.GetAccount(req.SessionKey).MultiFloorRaids.LastOrDefault() ?? new(),
                ParcelResultDB = ParcelService.GetParcelResult(account)
            };
        }
    }
}
