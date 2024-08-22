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
        public static void ConsumeCurrencies(ref AccountDB account, List<long> costIdList, List<int> costAmountList)
        {
            if(costIdList.Count !=  costAmountList.Count)
                return;

            var currencyDict = account.Currencies.First();
            for (int i = 0; i < costIdList.Count; i++)
            {
                currencyDict.CurrencyDict[(CurrencyTypes)costIdList[i]] -= costAmountList[i];
                currencyDict.UpdateTimeDict[(CurrencyTypes)costIdList[i]] = DateTime.Now;
            }
        }

        public static void ConsumeGem(ref AccountDB account, CurrencyTransaction transaction)
        {
            var currencyDict = account.Currencies.First().CurrencyDict;

            if(transaction.Gem > 0) // consume both paid gem and bonus gem
            {
                currencyDict[CurrencyTypes.GemPaid] -= transaction.Gem;

                if (currencyDict[CurrencyTypes.GemPaid] < 0)
                {
                    currencyDict[CurrencyTypes.GemBonus] += currencyDict[CurrencyTypes.GemPaid];
                    currencyDict[CurrencyTypes.GemPaid] = 0;
                }
            }

            if(transaction.GemPaid > 0)
                currencyDict[CurrencyTypes.GemPaid] -= transaction.GemPaid;

            if (transaction.GemBonus > 0)
                currencyDict[CurrencyTypes.GemBonus] -= transaction.GemBonus;
        }
    }
}
