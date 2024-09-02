using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCHALE.Common.MX.Campaign.HexaTileMapEvent.HexaTileMapCondition
{
    public abstract class HexaCondition
    {
        public long ConditionId { get; set; }
        public abstract HexaConditionType Type { get; }
        public abstract bool Resuable { get; }
        public bool AlreadyTriggered { get; set; }
    }

    public enum HexaConditionType
    {
        None = 0,
        StartCampaign = 1,
        TurnBeginEnd = 2,
        UnitDead = 3,
        PlayerArrivedInTileFirstTime = 4,
        AnyEnemyDead = 5,
        EveryTurn = 6,
        EnemyArrivedInTileFirstTime = 7,
        SpecificEnemyArrivedInTileFirstTime = 8,
    }
}
