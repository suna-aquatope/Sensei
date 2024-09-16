using SCHALE.Common.Database;
using SCHALE.Common.Database.ModelExtensions;
using SCHALE.Common.FlatData;
using SCHALE.Common.Migrations;
using SCHALE.GameServer.Services;
using SCHALE.GameServer.Services.Irc;
using System;
using System.Data;

namespace SCHALE.Common.Utils
{
    public static class InventoryUtils
    {
        public static void AddAllCharacters(IrcConnection connection, bool maxed = true)
        {
            var account = connection.Account;
            var context = connection.Context;

            var characterExcel = connection.ExcelTableService.GetTable<CharacterExcelTable>().UnPack().DataList;
            var characterLevelExcel = connection.ExcelTableService.GetTable<CharacterLevelExcelTable>().UnPack().DataList;
            var favorLevelExcel = connection.ExcelTableService.GetTable<FavorLevelExcelTable>().UnPack().DataList;

            var allCharacters = characterExcel.Where(x =>
                x is
                {
                    IsPlayable: true,
                    IsPlayableCharacter: true,
                    IsNPC: false,
                    ProductionStep: ProductionStep.Release,
                }
            ).Select(x => {
                return new CharacterDB()
                {
                    UniqueId = x.Id,
                    StarGrade = maxed ? x.MaxStarGrade : x.DefaultStarGrade,
                    Level = maxed ? characterLevelExcel.Count : 1,
                    Exp = 0,
                    PublicSkillLevel = maxed ? 10 : 1,
                    ExSkillLevel = maxed ? 5 : 1,
                    PassiveSkillLevel = maxed ? 10 : 1,
                    ExtraPassiveSkillLevel = maxed ? 10 : 1,
                    LeaderSkillLevel = 1,
                    FavorRank = maxed ? favorLevelExcel.Count : 1,
                    IsNew = true,
                    IsLocked = true,
                    PotentialStats = { { 1, 0 }, { 2, 0 }, { 3, 0 } },
                    EquipmentServerIds = [0, 0, 0]
                };
            }).ToList();

            account.AddCharacters(context, [.. allCharacters]);
            context.SaveChanges();

            connection.SendChatMessage("Added all characters!");
        }

        public static void AddAllEquipment(IrcConnection connection/*, bool maxed = true*/)
        {
            var equipmentExcel = connection.ExcelTableService.GetTable<EquipmentExcelTable>().UnPack().DataList;

            var allEquipment = equipmentExcel.Select(x =>
            {
                return new EquipmentDB()
                {
                    UniqueId = x.Id,
                    Level = 1,
                    StackCount = x.StackableMax, // ~ 90,000 cap, auto converted if over
                };
            }).ToList();

            connection.Account.AddEquipment(connection.Context, [.. allEquipment]);
            connection.Context.SaveChanges();

            // current implementation doesn't work
            /*var characterExcel = connection.ExcelTableService.GetTable<CharacterExcelTable>().UnPack().DataList;
            var allCharacterEquipment = characterExcel.FindAll(x => connection.Account.Characters.Any(y => y.UniqueId == x.Id)).ToList();
            foreach (var characterEquipmentData in allCharacterEquipment)
            {
                var characterEquipment = characterEquipmentData.EquipmentSlot.Select(x =>
                {
                    var equipmentData = equipmentExcel.FirstOrDefault(y => y.EquipmentCategory == x && y.MaxLevel == 65 && y.RecipeId == 0 && y.TierInit == 9);
                    return new EquipmentDB()
                    {
                        UniqueId = equipmentData.Id,
                        Level = maxed ? equipmentData.MaxLevel : 0,
                        Tier = maxed ? (int)equipmentData.TierInit : 1,
                        StackCount = 1,
                        BoundCharacterServerId = connection.Account.Characters.FirstOrDefault(y => y.UniqueId == characterEquipmentData.Id).ServerId
                    };
                });
                connection.Account.AddEquipment(connection.Context, [.. characterEquipment]);
                connection.Context.SaveChanges();
                connection.Account.Characters.FirstOrDefault(x => x.UniqueId == characterEquipmentData.Id).EquipmentServerIds.Clear();
                connection.Account.Characters.FirstOrDefault(x => x.UniqueId == characterEquipmentData.Id).EquipmentServerIds.AddRange(characterEquipment.Select(x => x.ServerId));
            }
            connection.Context.SaveChanges();
            */

            connection.SendChatMessage("Added all equipment!");
        }

        public static void AddAllItems(IrcConnection connection)
        {
            var itemExcel = connection.ExcelTableService.GetTable<ItemExcelTable>().UnPack().DataList;
            var allItems = itemExcel.Select(x =>
            {
                return new ItemDB()
                {
                    IsNew = true,
                    UniqueId = x.Id,
                    StackCount = x.StackableMax
                };
            }).ToList();

            connection.Account.AddItems(connection.Context, [.. allItems]);
            connection.Context.SaveChanges();

            connection.SendChatMessage("Added all items!");
        }

        public static void AddAllWeapons(IrcConnection connection, bool maxed = true)
        {
            var account = connection.Account;
            var context = connection.Context;

            var weaponExcel = connection.ExcelTableService.GetTable<CharacterWeaponExcelTable>().UnPack().DataList;
            // only for current characters
            var allWeapons = account.Characters.Select(x =>
            {
                return new WeaponDB()
                {
                    UniqueId = x.UniqueId,
                    BoundCharacterServerId = x.ServerId,
                    IsLocked = !maxed,
                    StarGrade = maxed ? weaponExcel.FirstOrDefault(y => y.Id == x.UniqueId).Unlock.TakeWhile(y => y).Count() : 1,
                    Level = maxed ? 50 : 1
                };
            });

            account.AddWeapons(context, [.. allWeapons]);
            context.SaveChanges();

            connection.SendChatMessage("Added all weapons!");
        }

        public static void AddAllGears(IrcConnection connection, bool maxed = true)
        {
            var account = connection.Account;
            var context = connection.Context;

            if(!maxed) return;
            var uniqueGearExcel = connection.ExcelTableService.GetTable<CharacterGearExcelTable>().UnPack().DataList;

            var uniqueGear = uniqueGearExcel.Where(x => x.Tier == 2 && context.Characters.Any(y => y.UniqueId == x.CharacterId)).Select(x => new GearDB()
            {
                UniqueId = x.Id,
                Level = 1,
                SlotIndex = 4,
                BoundCharacterServerId = context.Characters.FirstOrDefault(z => z.UniqueId == x.CharacterId).ServerId,
                Tier = maxed ? 2 : 1,
                Exp = 0,
            });

            account.AddGears(context, [.. uniqueGear]);
            context.SaveChanges();

            connection.SendChatMessage("Added all gears!");
        }

        public static void AddAllMemoryLobbies(IrcConnection connection)
        {
            var account = connection.Account;
            var context = connection.Context;

            var memoryLobbyExcel = connection.ExcelTableService.GetExcelList<MemoryLobbyExcel>("MemoryLobbyDBSchema");
            var allMemoryLobbies = memoryLobbyExcel.Select(x =>
            {
                return new MemoryLobbyDB()
                {
                    MemoryLobbyUniqueId = x.Id,
                };
            }).ToList();

            account.AddMemoryLobbies(context, [.. allMemoryLobbies]);
            context.SaveChanges();

            connection.SendChatMessage("Added all Memory Lobbies!");
        }

        public static void AddAllScenarios(IrcConnection connection)
        {
            var account = connection.Account;
            var context = connection.Context;

            var scenarioModeExcel = connection.ExcelTableService.GetExcelList<ScenarioModeExcel>("ScenarioModeDBSchema");
            var allScenarios = scenarioModeExcel.Select(x =>
            {
                return new ScenarioHistoryDB()
                {
                    ScenarioUniqueId = x.ModeId,
                };
            }).ToList();

            account.AddScenarios(context, [.. allScenarios]);
            context.SaveChanges();

            connection.SendChatMessage("Added all Scenarios!");
        }

        public static void RemoveAllCharacters(IrcConnection connection) // removing default characters breaks game
        {
            var characterDB = connection.Context.Characters;
            var defaultCharacters = connection.ExcelTableService.GetTable<DefaultCharacterExcelTable>().UnPack().DataList.Select(x => x.CharacterId).ToList();

            var removed = characterDB.Where(x => x.AccountServerId == connection.AccountServerId && !defaultCharacters.Contains(x.UniqueId));

            characterDB.RemoveRange(removed);

            connection.SendChatMessage("Removed all characters!");
        }

        public static CharacterDB CreateMaxCharacterFromId(uint characterId)
        {
            return new CharacterDB()
            {
                UniqueId = characterId,
                StarGrade = 5,
                Level = 90,
                Exp = 0,
                PublicSkillLevel = 10,
                ExSkillLevel = 5,
                PassiveSkillLevel = 10,
                ExtraPassiveSkillLevel = 10,
                LeaderSkillLevel = 1,
                FavorRank = 100,
                IsNew = true,
                IsLocked = true,
                PotentialStats = { { 1, 0 }, { 2, 0 }, { 3, 0 } },
                EquipmentServerIds = [0, 0, 0]
            };
        }
    }
}
