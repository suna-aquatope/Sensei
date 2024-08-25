using SCHALE.Common.Database;
using SCHALE.Common.FlatData;
using SCHALE.Common.Migrations.SqlServerMigrations;
using SCHALE.Common.NetworkProtocol;
using SCHALE.Common.Parcel;
using SCHALE.GameServer.Controllers.Api;
using SCHALE.GameServer.Services.Irc;

namespace SCHALE.GameServer.Services
{
    public class CurrencyUtils
    {
        public static void ChangeCurrencies(ref AccountDB account, List<long> costIdList, List<int> costAmountList, bool isReturn = false)
        {
            if(costIdList.Count != costAmountList.Count)
                return;

            var currencyDict = account.Currencies.First();
            for (int i = 0; i < costIdList.Count; i++)
            {
                var targetCurrencyType = (CurrencyTypes)costIdList[i];
                currencyDict.CurrencyDict[targetCurrencyType] += costAmountList[i] * (isReturn ? 1 : -1);
                currencyDict.UpdateTimeDict[targetCurrencyType] = DateTime.Now;
            }
        }

        public static void ConsumeCurrencies(ref AccountDB account, CurrencyTransaction transaction)
        {
            var currencyDict = account.Currencies.First().CurrencyDict;

            foreach(var currencyPair in transaction.currencyValue.Values)
            {
                switch(currencyPair.Key)
                {
                    case CurrencyTypes.Gem:
                        currencyDict[CurrencyTypes.GemPaid] -= currencyPair.Value;

                        if (currencyDict[CurrencyTypes.GemPaid] < 0)
                        {
                            currencyDict[CurrencyTypes.GemBonus] += currencyDict[CurrencyTypes.GemPaid];
                            currencyDict[CurrencyTypes.GemPaid] = 0;
                        }
                        break;

                    default:
                        currencyDict[currencyPair.Key] -= currencyPair.Value;
                        break;
                }
            }
        }
    }
}
