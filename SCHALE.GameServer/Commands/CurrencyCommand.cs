using SCHALE.Common.Database;
using SCHALE.Common.FlatData;
using SCHALE.Common.Utils;
using SCHALE.GameServer.Services;
using SCHALE.GameServer.Services.Irc;

namespace SCHALE.GameServer.Commands
{
    [CommandHandler("currency", "Command to manage currency (gem, ticket)", "/currency [currencyId] [amount]")]
    internal class CurrencyCommand : Command
    {
        public CurrencyCommand(IrcConnection connection, string[] args, bool validate = true) : base(connection, args, validate) { }

        [Argument(0, @"", "The id of currency you want to change its amount", ArgumentFlags.IgnoreCase)]
        public string id { get; set; } = string.Empty;

        [Argument(1, @"", "amount", ArgumentFlags.IgnoreCase)]
        public string amountStr { get; set; } = string.Empty;

        public override void Execute()
        {
            var currencyType = CurrencyTypes.Invalid;
            long amount = 0;
            if(Enum.TryParse<CurrencyTypes>(id, true, out currencyType) && currencyType != CurrencyTypes.Invalid && Int64.TryParse(amountStr, out amount))
            {
                connection.Account.Currencies.First().CurrencyDict[currencyType] = amount;
                connection.Account.Currencies.First().UpdateTimeDict[currencyType] = DateTime.Now;
                connection.Context.SaveChanges();

                connection.SendChatMessage($"Set amount of {currencyType.ToString()} to {amount}!");
            } else
            {
                throw new ArgumentException("Invalid Target / Amount!");
            }
        }
    }
}
