using SCHALE.Common.Database;
using SCHALE.Common.FlatData;

public class ScoreService
{
    public static Dictionary<StarGoalType, long> CalculateScore(BattleSummary summary, List<StarGoalType> starGoalTypes, List<int> starGoalAmounts)
    {
        if(starGoalTypes.Count != starGoalAmounts.Count)
        {
            return new();
        }

        Dictionary<StarGoalType, long> stars = new();

        for (int i = 0; i < starGoalTypes.Count; i++)
        {
            var targetGoalType = starGoalTypes[i];
            var targetGoalAmount = starGoalAmounts[i];

            switch (targetGoalType)
            {
                case StarGoalType.Clear:
                    if(summary.EndType == SCHALE.Common.MX.Logic.Battles.BattleEndType.Clear)
                    {
                        stars.Add(targetGoalType, 1);
                    }
                    break;

                case StarGoalType.AllAlive:
                    if (/*req.Summary.*/ true) // how can i judge if all characters are alived?
                    {
                        stars.Add(targetGoalType, 1);
                    }
                    break;

                case StarGoalType.GetBoxes:
                    stars.Add(targetGoalType, summary.WeekDungeonSummary.FindGifts.First().ClearCount);
                    return stars;

                case StarGoalType.ClearTimeInSec:
                    if (summary.EndFrame <= targetGoalAmount * 30)
                    {
                        stars.Add(targetGoalType, 1);
                    }
                    break;
            }
        }

        return stars;
    }
}