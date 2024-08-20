using SCHALE.Common.Database;
using SCHALE.Common.NetworkProtocol;
using SCHALE.Common.Parcel;
using SCHALE.GameServer.Controllers.Api;

namespace SCHALE.GameServer.Services
{
    public class ParcelService
    {
        private readonly Dictionary<long, Guid> sessions = [];
        private readonly SCHALEContext context;

        public ParcelService(SCHALEContext _context)
        {
            context = _context;
        }

        public static ParcelResultDB GetParcelResult(AccountDB account, long baseExp = 0, long addExp = 0)
        {
            return new ParcelResultDB()
            {
                AccountDB = account,
                AcademyLocationDBs = new(),
                AccountCurrencyDB = account.Currencies.First(),
                CharacterDBs = account.Characters.ToList(),
                WeaponDBs = account.Weapons.ToList(),
                CostumeDBs = new(),
                TSSCharacterDBs = new(),
                EquipmentDBs = new(),
                RemovedEquipmentIds = new(),
                ItemDBs = new(),
                RemovedItemIds = new(),
                FurnitureDBs = new(),
                RemovedFurnitureIds = new(),
                IdCardBackgroundDBs = new(),
                EmblemDBs = new(),
                StickerDBs = new(),
                MemoryLobbyDBs = account.MemoryLobbies.ToList(),
                CharacterNewUniqueIds = new(),
                SecretStoneCharacterIdAndCounts = new(),
                DisplaySequence = new(),
                ParcelForMission = new(),
                ParcelResultStepInfoList = new(),
                BaseAccountExp = baseExp,
                AdditionalAccountExp = addExp,
                GachaResultCharacters = new(),
            };
        }
    }
}
