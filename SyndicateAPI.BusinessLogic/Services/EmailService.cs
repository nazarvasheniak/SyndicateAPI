using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Utils.Helpers;
using System.Threading.Tasks;

namespace SyndicateAPI.BusinessLogic.Services
{
    public class EmailService : IEmailService
    {
        public async Task<bool> SendActivationMessage(string to, int code)
        {
            var message = new SendMailModel
            {
                MailTo = to,
                Subject = "[Syndicate] Активация",
                Message = $"Ваш код активации: {code}"
            };

            return await SMTPHelper.SendMail(message);
        }

        public async Task<bool> SendChangeMessage(string to, int code)
        {
            var message = new SendMailModel
            {
                MailTo = to,
                Subject = "[Syndicate] Активация",
                Message = $"Подтвердите изменение Email.<br>Ваш код подтверждения: {code}"
            };

            return await SMTPHelper.SendMail(message);
        }
    }
}
