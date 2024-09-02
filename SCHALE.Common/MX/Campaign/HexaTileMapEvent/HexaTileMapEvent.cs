using SCHALE.Common.FlatData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCHALE.Common.MX.Campaign.HexaTileMapEvent.HexaTileMapCondition;
using SCHALE.Common.MX.Campaign.HexaTileMapEvent.HexaTileMapCommand;

namespace SCHALE.Common.MX.Campaign.HexaTileMapEvent
{
    public class HexaEvent
    {
        public string EventName { get; set; }
        public long EventId { get; set; }
        public IList<HexaCondition> HexaConditions { get; }
        public MultipleConditionCheckType MultipleConditionCheckType { get; set; }
        public IList<HexaCommand> HexaCommands { get; }
    }
}
