using SCHALE.Common.Database;
using SCHALE.Common.FlatData;
using SCHALE.Common.NetworkProtocol;
using SCHALE.Common.Parcel;
using SCHALE.GameServer.Controllers.Api;
using SCHALE.GameServer.Services.Irc;

namespace SCHALE.GameServer.Services
{
    public class CurrencyUtils
    {
        public static void ConsumeCurrencies(ref AccountDB account, Dictionary<CurrencyTypes, long> keys)
        {
            foreach (var kvp in keys)
            {
                account.Currencies.First().CurrencyDict[kvp.Key] -= kvp.Value;
                account.Currencies.First().UpdateTimeDict[kvp.Key] = DateTime.Now;
            }
        }

        public static void ConsumeGem(ref AccountDB account, long consumeAmount)
        {
            var currencyDict = account.Currencies.First().CurrencyDict;
            currencyDict[CurrencyTypes.GemPaid] -= consumeAmount;

            if(currencyDict[CurrencyTypes.GemPaid] < 0)
            {
                currencyDict[CurrencyTypes.GemBonus] += currencyDict[CurrencyTypes.GemPaid];
                currencyDict[CurrencyTypes.GemPaid] = 0;
            }
        }
    }
}
