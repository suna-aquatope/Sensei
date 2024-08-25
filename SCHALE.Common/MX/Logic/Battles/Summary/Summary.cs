using SCHALE.Common.FlatData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

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
}
