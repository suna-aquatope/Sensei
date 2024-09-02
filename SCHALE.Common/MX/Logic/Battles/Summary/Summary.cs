using SCHALE.Common.FlatData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using SCHALE.Common.Database;
using System.Collections.ObjectModel;

namespace SCHALE.Common.MX.Logic.Battles.Summary
{
    public class FindGiftSummary
    {
        public string UniqueName { get; set; }
        public int ClearCount { get; set; }
    }

    public class WeekDungeonSummary : IEquatable<WeekDungeonSummary>
    {
        public WeekDungeonType DungeonType { get; set; }
        public List<FindGiftSummary> FindGifts { get; set; }
        [JsonIgnore]
        public int TotalFindGiftClearCount { get; }

        public virtual bool Equals(WeekDungeonSummary other) {
            return other == this;
        }
    }

    public class GroupSummary : IEquatable<GroupSummary>
    {
        public long TeamId { get; set; }
        public EntityId LeaderEntityId { get; set; }
        [JsonIgnore]
        public long LeaderCharacterId { get; }
        // Thanks https://github.com/suna-aquatope
        public List<HeroSummary>/*KeyedCollection<EntityId, HeroSummary>*/ Heroes { get; set; }
        public List<HeroSummary>/*KeyedCollection<EntityId, HeroSummary>*/ Supporters { get; set; }
        [JsonIgnore]
        public int AliveCount { get; }
        public bool UseAutoSkill { get; set; }
        public long TSSInteractionServerId { get; set; }
        public long TSSInteractionUniqueId { get; set; }
        public Dictionary<long, AssistRelation> AssistRelations { get; set; }
        [JsonIgnore]
        public int StrikerMaxLevel { get; }
        [JsonIgnore]
        public int SupporterMaxLevel { get; }
        [JsonIgnore]
        public int StrikerMinLevel { get; }
        [JsonIgnore]
        public int SupporterMinLevel { get; }
        [JsonIgnore]
        public int MaxCharacterLevel { get; }
        [JsonIgnore]
        public int MinCharacterLevel { get; }
        [JsonIgnore]
        public long TotalDamageGivenApplied { get; }
        //public SkillCostSummary SkillCostSummary { get; set; }

        public bool Equals(GroupSummary other) {
            return other == this;
        }
    }

    [Serializable]
    public struct EntityId : IComparable, IComparable<EntityId>, IEquatable<EntityId>
    {
        private const uint typeMask = 4278190080;
        private const int instanceIdMask = 16777215;

        public static EntityId Invalid { get; }
        [JsonIgnore]
        public BattleEntityType EntityType { get; }
        [JsonIgnore]
        public int InstanceId { get; }
        [JsonIgnore]
        public int UniqueId { get; }
        [JsonIgnore]
        public bool IsValid { get; }

        public int CompareTo(object obj) {
            return this.CompareTo(obj);
        }

        public int CompareTo(EntityId other) {
            return this.CompareTo(other);
        }

        public bool Equals(EntityId other) {
            return this.Equals(other);
        }
    }

    //[JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum BattleEntityType
    {
        None = 0,
        Character = 16777216,
        SkillActor = 33554432,
        Obstacle = 67108864,
        Point = 134217728,
        Projectile = 268435456,
        EffectArea = 536870912,
        Supporter = 1073741824,
        BattleItem = -2147483648,
    }

    public class HeroSummary// : IEquatable<HeroSummary>
    {
        public long ServerId { get; set; }
        public long OwnerAccountId { get; set; }
        public EntityId BattleEntityId { get; set; }
        public long CharacterId { get; set; }
        public long CostumeId { get; set; }
        public int Grade { get; set; }
        public int Level { get; set; }
        public IDictionary<StatType, int> PotentialStatLevel { get; set; }
        public int ExSkillLevel { get; set; }
        public int PublicSkillLevel { get; set; }
        public int PassiveSkillLevel { get; set; }
        public int ExtraPassiveSkillLevel { get; set; }
        public int FavorRank { get; set; }
        //public StatSnapshotCollection StatSnapshotCollection { get; set; }
        public long HPRateBefore { get; set; }
        public long HPRateAfter { get; set; }
        public int CrowdControlCount { get; set; }
        public int CrowdControlDuration { get; set; }
        public int EvadeCount { get; set; }
        public int DamageImmuneCount { get; set; }
        public int CrowdControlImmuneCount { get; set; }
        public long MaxAttackPower { get; set; }
        public int AverageCriticalRate { get; set; }
        public int AverageStabilityRate { get; set; }
        public int AverageAccuracyRate { get; set; }
        public int DeadFrame { get; set; }
        public long DamageGivenAbsorbedSum { get; set; }
        public TacticEntityType TacticEntityType { get; set; }
        [JsonIgnore]
        public HeroSummaryDetailFlag DetailFlag { get; }
        [JsonIgnore]
        public bool IsDead { get; }
        //public List<BattleNumericLog> GivenNumericLogs { get; set; }
        //public List<BattleNumericLog> TakenNumericLogs { get; set; }
        //public List<BattleNumericLog> ObstacleBattleNumericLogs { get; set; }
        public List<EquipmentSetting> Equipments { get; set; }
        public Nullable<WeaponSetting> CharacterWeapon { get; set; }
        [JsonIgnore]
        public IDictionary<int, long> HitPointByFrame { get; set; }
        public IDictionary<SkillSlot, int> SkillCount { get; set; }
        [JsonIgnore]
        public int ExSkillUseCount { get; }
        //public KillLogCollection KillLog { get; set; }
        [JsonIgnore]
        public int KillCount { get; }
        [JsonIgnore]
        public Dictionary<int, string> FullSnapshot { get; set; }
        public static IEqualityComparer<HeroSummary> HeroSummaryAlmostEqualityComparer { get; }

        public bool Equals(HeroSummary other) {
            return this.Equals(other);
        }
    }

    public class StatSnapshotCollection : KeyedCollection<StatType, StatSnapshot>
    {
        protected override StatType GetKeyForItem(StatSnapshot item) {
            return this.GetKeyForItem(item);
        }
    }

    public class StatSnapshot
    {
        public StatType Stat { get; set; }
        public long Start { get; set; }
        public long End { get; set; }
        [JsonIgnore]
        public long Diff { get; }
    }

    [Flags]
    public enum HeroSummaryDetailFlag
    {
        None = 0,
        BattleProperty = 2,
        BattleStatistics = 4,
        NumericLogs = 8,
        StatSnapshot = 16,
        Default = 14,
        All = 30,
    }

    public class BattleNumericLog : IEquatable<BattleNumericLog>
    {
        public BattleEntityType EntityType { get; set; }
        public BattleLogCategory Category { get; set; }
        public BattleLogSourceType Source { get; set; }
        public long CalculatedSum { get; set; }
        public long AppliedSum { get; set; }
        public long Count { get; set; }
        public long CriticalMultiplierMax { get; set; }
        public long CriticalCount { get; set; }
        public long CalculatedMin { get; set; }
        public long CalculatedMax { get; set; }
        public long AppliedMin { get; set; }
        public long AppliedMax { get; set; }

        public bool Equals(BattleNumericLog other) {
            return this.Equals(other);
        }
    }

    public enum BattleLogCategory
    {
        None = 0,
        Damage = 1,
        Heal = 2,
    }

    public enum BattleLogSourceType
    {
        None = 0,
        Normal = 1,
        Ex = 2,
        Public = 3,
        Passive = 4,
        ExtraPassive = 5,
        Etc = 6
    }

    public struct EquipmentSetting : IEquatable<EquipmentSetting>
    {
        public const int InvalidId = -1;

        [JsonIgnore]
        public bool IsValid { get; }
        public long ServerId { get; set; }
        public long UniqueId { get; set; }
        public int Level { get; set; }
        public int Tier { get; set; }

        public bool Equals(EquipmentSetting other) {
            return this.Equals(other);
        }
    }

    public struct WeaponSetting : IEquatable<WeaponSetting>
    {
        public const int InvalidId = -1;

        [JsonIgnore]
        public bool IsValid { get; }
        public long UniqueId { get; set; }
        public int StarGrade { get; set; }
        public int Level { get; set; }

        public bool Equals(WeaponSetting other) {
            return this.Equals(other);
        }
    }
}
