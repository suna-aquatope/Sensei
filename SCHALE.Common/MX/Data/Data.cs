using SCHALE.Common.FlatData;
using SCHALE.Common.Parcel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SCHALE.Common.MX.Campaign;

namespace SCHALE.Common.MX.Data
{
    public class CampaignStageInfo
    {
        public long UniqueId { get; set; }
        public string DevName { get; set; }
        public long ChapterNumber { get; set; }
        public string StageNumber { get; set; }
        public long TutorialStageNumber { get; set; }
        public long[] PrerequisiteScenarioIds { get; set; }
        public int RecommandLevel { get; set; }
        public string StrategyMap { get; set; }
        public string BackgroundBG { get; set; }
        public long StoryUniqueId { get; set; }
        public long ChapterUniqueId { get; set; }
        public long DailyPlayCountLimit { get; }
        public StageTopography StageTopography { get; set; }
        public int StageEnterCostAmount { get; set; }
        public int MaxTurn { get; set; }
        public int MaxEchelonCount { get; set; }
        public StageDifficulty StageDifficulty { get; set; }
        public HashSet<long> PrerequisiteStageUniqueIds { get; set; }
        public long DailyPlayLimit { get; set; }
        public TimeSpan PlayTimeLimit { get; set; }
        public long PlayTurnLimit { get; set; }
        public ParcelCost EnterCost { get; }
        public ParcelCost PurchasePlayCountHardStageCost { get; set; }
        public HexaTileMap HexaTileMap { get; set; }
        public long StarConditionTurnCount { get; set; }
        public long StarConditionSTacticRackCount { get; set; }
        public long RewardUniqueId { get; set; }
        public long TacticRewardPlayerExp { get; set; }
        public long TacticRewardExp { get; set; }
        public virtual bool ShowClearDeckButton { get; }
        public List<ValueTuple<ParcelInfo, RewardTag>> StageReward { get; set; }
        public List<ValueTuple<ParcelInfo, RewardTag>> DisplayReward { get; set; }
        public StrategyEnvironment StrategyEnvironment { get; set; }
        public ContentType ContentType { get; set; }
        public long GroundId { get; set; }
        public int StrategySkipGroundId { get; }
        public long BattleDuration { get; set; }
        public long BGMId { get; set; }
        public long FixedEchelonId { get; set; }
        public bool IsEventContent { get; }
        public ParcelInfo EnterParcelInfo { get; set; }
        public bool IsDeprecated { get; set; }
        public EchelonExtensionType EchelonExtensionType { get; set; }
    }
}
