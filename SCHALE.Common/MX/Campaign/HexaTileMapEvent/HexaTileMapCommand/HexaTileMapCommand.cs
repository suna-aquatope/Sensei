using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCHALE.Common.MX.Campaign.HexaTileMapEvent.HexaTileMapCommand
{
    public abstract class HexaCommand
    {
        public long CommandId { get; set; }
        public virtual HexaCommandType Type { get; }
    }

    public enum HexaCommandType
    {
        None = 0,
        UnitSpawn = 1,
        PlayScenario = 2,
        StrategySpawn = 3,
        TileSpawn = 4,
        TileHide = 5,
        EndBattle = 6,
        WaitTurn = 7,
        StrategyHide = 8,
        UnitDie = 9,
        UnitMove = 10,
        CharacterEmoji = 11,
    }
}
