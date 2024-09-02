using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using SCHALE.Common.MX.Campaign.HexaTileMapEvent;
using SCHALE.Common.FlatData;
using SCHALE.Common.Parcel;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace SCHALE.Common.MX.Campaign
{
    public class HexaTileMap
    {
        public static readonly float XOffset;
        public static readonly float YOffset;
        public static readonly float EmptyOffset;
        public static readonly float Up;
        public int LastEntityId;
        public bool IsBig;
        private List<HexaEvent> events;
        public List<HexaTile> hexaTileList;
        public List<HexaUnit> hexaUnitList;
        public List<Strategy> hexaStrageyList;
        [JsonIgnore]
        public Dictionary<HexLocation, HexaTile> TileLocationMap;
    }

    [Serializable]
    public class HexaTile
    {
        public string ResourcePath;
        public bool IsHide;
        public bool IsFog;
        public bool CanNotMove;
        public HexLocation Location;
        public Strategy Strategy;
        public HexaUnit Unit;
	    [JsonIgnore]
        public HexaUnit ChallengeUnit;

        public bool PlayAnimation { get; set; }
        [JsonIgnore]
        public bool IsBattleReady { get; }
        [JsonIgnore]
        public bool StartTile { get; }
    }

    public class HexaUnit
    {
        public long EntityId;
        public Dictionary<long, long> HpInfos;
        public Dictionary<long, long> DyingInfos;
        public Dictionary<long, int> BuffInfos;
        public int ActionCountMax;
        public int ActionCount;
        public int Mobility;
        public int StrategySightRange;
        public long Id;
        public Vector3 Rotate;
        public HexLocation Location;
        public HexLocation AIDestination;
        public bool IsActionComplete;
        public bool IsPlayer;
        public bool IsFixedEchelon;
        public int MovementOrder;
	    public Dictionary<TacticEntityType, List<ParcelInfo>> RewardParcelInfosWithDropTacticEntityType;
        [JsonIgnore]
        public CampaignUnitExcel CampaignUnitExcel;
        [JsonIgnore]
        public List<HexaTile> MovableTiles;
        [JsonIgnore]
        public List<List<HexaTile>> MovementMap;

        [JsonIgnore]
        public List<string> BuffGroupIds { get; }
        public SkillCardHand SkillCardHand { get; set; }
        public bool PlayAnimation { get; set; }
        [JsonIgnore]
        public Dictionary<TacticEntityType, List<long>> RewardItems { get; }
    }

    [Serializable]
    public struct HexLocation : IEquatable<HexLocation>
    {
        public int x;
        public int y;
        public int z;
        [JsonIgnore]
        public static readonly int NeighborCount;
        [JsonIgnore]
        public static readonly HexLocation[] Directions;

        [JsonIgnore]
        public static HexLocation Zero { get; }
        [JsonIgnore]
        public static HexLocation Invalid { get; }

        public bool Equals(HexLocation other) {
            return this.Equals(other);
        }
    }

    public class SkillCardHand
    {
        public float Cost { get; set; }
        public List<SkillCardInfo> SkillCardsInHand { get; set; }
    }

    public struct SkillCardInfo
    {
        public long CharacterId { get; set; }
        public int HandIndex { get; set; }
        public string SkillId { get; set; }
        public int RemainCoolTime { get; set; }
    }

    public class Strategy
    {
        public long EntityId;
        public Vector3 Rotate;
        public long Id;
        public HexLocation Location;
	    [JsonIgnore]
        public CampaignStrategyObjectExcel CampaignStrategyExcel;
    }
}
