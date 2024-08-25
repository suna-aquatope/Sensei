using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCHALE.Common.MX.Logic.Data
{
    [Flags]
    public enum BattleTypes
    {
        None = 0,
        Adventure = 1,
        ScenarioMode = 2,
        WeekDungeonChaserA = 4,
        WeekDungeonBlood = 8,
        WeekDungeonChaserB = 16,
        WeekDungeonChaserC = 32,
        WeekDungeonFindGift = 64,
        EventContent = 128,
        TutorialAdventure = 256,
        Profiling = 512,
        SingleRaid = 2048,
        MultiRaid = 4096,
        PracticeRaid = 8192,
        EliminateRaid = 16384,
        MultiFloorRaid = 32768,
        MinigameDefense = 1048576,
        Arena = 2097152,
        TimeAttack = 8388608,
        SchoolDungeonA = 33554432,
        SchoolDungeonB = 67108864,
        SchoolDungeonC = 134217728,
        WorldRaid = 268435456,
        Conquest = 536870912,
        FieldStory = 1073741824,
        FieldContent = -2147483648,
        PvE = -301988865,
        WeekDungeon = 124,
        SchoolDungeon = 234881024,
        Raid = 30720,
        PvP = 2097152,
        All = -1,
    }
}
