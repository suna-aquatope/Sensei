using SCHALE.Common.Database;
using SCHALE.Common.FlatData;
using SCHALE.Common.NetworkProtocol;
using SCHALE.GameServer.Services;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Mail : ProtocolHandlerBase
    {
        private ISessionKeyService sessionKeyService;
        private SCHALEContext context;
		
		public Mail(IProtocolHandlerFactory protocolHandlerFactory, ISessionKeyService _sessionKeyService, SCHALEContext _context) : base(protocolHandlerFactory) 
		{
			sessionKeyService = _sessionKeyService;
            context = _context;
		}

        [ProtocolHandler(Protocol.Mail_Check)]
        public ResponsePacket CheckHandler(MailCheckRequest req)
        {

			return new MailCheckResponse()
            {
				
            };
        }
		
		[ProtocolHandler(Protocol.Mail_List)]
		public ResponsePacket ListHandler(MailListRequest req)
		{
			var account = sessionKeyService.GetAccount(req.SessionKey);
			var mailDb = account.Mails.FirstOrDefault();
			return new MailListResponse()
			{
				MailDB = mailDb,
				MailDBs = account.Mails.ToList()
			};
		}
		
		public static MailDB CreateMail(long accountId)
        {
            return new()
            {
                AccountServerId = accountId,
                Type = SCHALE.Common.FlatData.MailType.System,
                UniqueId = 1,
                Sender = "Arona",
                Comment = "This is test, Sensei~",
                SendDate = DateTime.Now,
                ExpireDate = DateTime.MaxValue,
            };
        }


    }
}
